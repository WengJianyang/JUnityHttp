using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace CommonTools.JUnityHttp.Cache
{
    /// <summary>
    /// Default cache for JUnityHttp
    /// </summary>
    public class HttpCacheDefault : MonoBehaviour, IHttpCache
    {
        public static string fileName = "cac.hdd";

        private static HttpCacheDefault _instance;
        private static bool _forceDestroy = false;

        /// <summary>
        /// cache  version 
        /// </summary>
        public string version = "0";
        
        
        public static Func<bool, string> pathProvider = null;
        public static Func<string, string> encryptProvider = null;
        public static Func<string, string> decryptProvider = null;

        public static HttpCacheDefault Instance
        {
            get
            {
                if (_instance == null)
                {
                    if (!_forceDestroy)
                    {
                        var g = new GameObject("JUnityHttpCache");
                        DontDestroyOnLoad(g);
                        _instance = g.AddComponent<HttpCacheDefault>();
                        if (_instance._cacheDatas == null)
                        {
                            _instance._cacheDatas = new Dictionary<string, CacheData>();
                        }
                    }
                }

                return _instance;
            }
        }

        public static void DestroyInstance()
        {
            if (_instance != null)
            {
                _instance = null;
                _forceDestroy = true;
            }
        }


        private Dictionary<string, CacheData> _cacheDatas;

        public bool TryGetCache(RequestBase request, out string data)
        {
            data = null;
            if (_cacheDatas.TryGetValue(request.hash, out CacheData cacheData))
            {
                if (!cacheData.m.IsCacheTimeOut())
                {
                    data = cacheData.t;
                    RequestBase.loger?.Log($"hit cache:{data}");
                }
                else
                {
                    RequestBase.loger?.Log($"cache time out :{data}");
                }
            }

            return data != null;
        }

        public void UpdateCache(string hash, string text, long age)
        {
            _cacheDatas[hash] = new CacheData()
            {
                m = CacheClock.current + age,
                h = hash,
                t = text
            };
            RequestBase.loger?.Log($"update cache:{hash}-{text}-{age}");
        }

        public void Clear()
        {
            _cacheDatas.Clear();
        }

        public void RemoveTimeOutData()
        {
            List<string> keys = new List<string>(_cacheDatas.Count);
            foreach (var data in _cacheDatas)
            {
                if (data.Value.m.IsCacheTimeOut())
                {
                    keys.Add(data.Key);
                }
            }

            foreach (var key in keys)
            {
                _cacheDatas.Remove(key);
            }
        }

        private static float _lastSaveTime;

        public void Save2Disk()
        {
            if (Time.time - _lastSaveTime > 1f)
            {
                _lastSaveTime = Time.time;
                RemoveTimeOutData();
                if (pathProvider != null)
                {
                    string dir = pathProvider(false);
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    string filePath = Path.Combine(dir, fileName);
                    string json = JsonUtility.ToJson(new VersionCacheData(version, _cacheDatas.Values.ToList()));
                    string str = encryptProvider?.Invoke(json) ?? json;
                    RequestBase.loger?.Log($"save:{str}");
                    File.WriteAllText(filePath, str);
                }
                else
                {
                    RequestBase.loger?.Log("pathProvider is null,HTTP cache only stays in memory");
                }
            }
        }
 
        public void LoadFormDisk(Action complete = null)
        {
            if (pathProvider != null)
            {
                string filePath = Path.Combine(pathProvider(true), fileName);
                new Get(filePath)
                    .SetTimeout(3)
                    .OnSuccess(_ =>
                    {
                        RequestBase.loger?.Log($"read:{_.text}");
                        string jsonStr = decryptProvider?.Invoke(_.text) ?? _.text;
                        var versionData = JsonUtility.FromJson<VersionCacheData>(jsonStr);
                        if (versionData.version == this.version)
                        {
                            foreach (var cacheData in versionData.datas)
                            {
                                _cacheDatas[cacheData.h] = cacheData;
                            }
                        }
                        else
                        {
                            RequestBase.loger?.Log("version difference , the local cached data will be remove");
                        }
                    })
                    .OnFailure(_ => RequestBase.loger?.Log($"read failed : {_.error}"))
                    .OnComplete(_ => { complete?.Invoke(); })
                    .Send();
            }
            else
            {
                RequestBase.loger?.LogWarning("pathProvider is null,HTTP cache can not read form disk");
                complete?.Invoke();
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                Save2Disk();
            }
        }

        private void OnDestroy()
        {
            Save2Disk();
        }
    }
}