using System.Collections.Generic;
using CommonTools.JUnityHttp;
using CommonTools.JUnityHttp.Cache;
using UnityEngine;

namespace CommonTools.JUnityHttpSample
{
    public class AppNetConfigSample : IHttpHeader, IHttpLog
    {
        private static AppNetConfigSample _configSample = null;

        public static void Init()
        {
            if (_configSample == null)
            {
                _configSample = new AppNetConfigSample();
            }

            //set cacher
            RequestBase.cacher = HttpCacheDefault.Instance;
            //using version ,if version change,old date of cache will be removed
            HttpCacheDefault.Instance.version = "0.0.1";
            //set global max age for each requset
            RequestBase.globalMaxAge = 3600000; //
            //Set cache path provider
            HttpCacheDefault.pathProvider = CachePath.CanReadAndWritePath;
            
            HttpCacheDefault.encryptProvider = CacheEnCode.Encrypt;
            HttpCacheDefault.decryptProvider = CacheEnCode.Decrypt;
            
            //setting the unified loger interface for all request
            RequestBase.loger = _configSample;
            //set global header
            RequestBase.httpHeader = _configSample;
        }

        static Dictionary<string, string> commonHeader = new Dictionary<string, string>()
        {
            {
                "token", "values"
            }
        };

        static Dictionary<string, string> weatherHeader = new Dictionary<string, string>()
        {
            {
                "Authorization", "APPCODE 70d20881c6e54725a5d2c63598c9cf64"
            }
        };

        public Dictionary<string, string> GetByHeader(string url)
        {
            if (url.StartsWith(ApiUrl.WeatherDoMain))
            {
                return weatherHeader;
            }

            return commonHeader;
        }

        public void Log(string text)
        {
            Debug.Log(text);
        }

        public void LogWarning(string text)
        {
            Debug.LogWarning(text);
        }

        public void LogError(string text)
        {
            //you can save it to your log-server
            Debug.LogError(text);
        }
    }
}