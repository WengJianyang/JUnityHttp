using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace CommonTools.JUnityHttp
{
    public abstract class RequestBase : IDisposable
    {
        public string url { get; protected set; } = null;
        public Dictionary<string, string> header = null;
        public UnityWebRequest www { get; protected set; } = null;
        public Dictionary<string, string> questData = new Dictionary<string, string>();

        public static int globalTimeout = 15; //seconds
        private int? _timeout = null;

        public static IHttpHeader httpHeader;
        public static IHttpLog loger;
        public static IHttpCache cacher;

        public string hash => $"{url}-{string.Join("-", questData)}";

        public int timeout
        {
            get
            {
                if (_timeout != null)
                {
                    return _timeout.Value;
                }

                return globalTimeout;
            }
        }

        public string error { get; protected set; } = null;
        public string text { get; protected set; } = null;
        public object result { get; protected set; } = null;
        public bool isSuccess { get; protected set; } = false;
        public bool isUsingCache { get; protected set; } = false;

        public long? _maxAge = null;
        public static long globalMaxAge = 72000; //2 hours

        public long maxAge
        {
            get
            {
                if (_maxAge != null)
                {
                    return _maxAge.Value;
                }

                return globalMaxAge;
            }
        }


        protected Action<RequestBase> onSuccess;
        protected Action<RequestBase> onFailure;

        protected Action<RequestBase> onPrepare;
        protected Action<RequestBase> onComplete;

        public RequestBase(string url)
        {
            this.url = url;
        }

        public RequestBase SetHeader(string key, object value)
        {
            if (header == null)
            {
                header = new Dictionary<string, string>();
            }

            header.Add(key, value.ToString());
            return this;
        }

        public RequestBase AddData(string key, object value)
        {
            if (questData == null)
            {
                questData = new Dictionary<string, string>();
            }

            questData.Add(key, value.ToString());
            return this;
        }

        public RequestBase AddData(Dictionary<string, object> values)
        {
            foreach (var value in values)
            {
                AddData(value.Key, value.Value);
            }

            return this;
        }

        public RequestBase OnSuccess(Action<RequestBase> action)
        {
            onSuccess = action;
            return this;
        }

        public RequestBase OnFailure(Action<RequestBase> action)
        {
            onFailure = action;
            return this;
        }

        public RequestBase OnPrepare(Action<RequestBase> action)
        {
            onPrepare = action;
            return this;
        }

        public RequestBase OnComplete(Action<RequestBase> action)
        {
            onComplete = action;
            return this;
        }

        public RequestBase UsingCache(bool isUseing = true)
        {
            isUsingCache = isUseing;
            return this;
        }

        protected virtual void Prepare()
        {
            if (httpHeader != null)
            {
                var tmpHeaders = httpHeader.GetByHeader(url);
                if (tmpHeaders != null && tmpHeaders.Count > 0)
                {
                    foreach (var keyValuePair in tmpHeaders)
                    {
                        SetHeader(keyValuePair.Key, keyValuePair.Value);
                    }
                }
            }

            onPrepare?.Invoke(this);
        }

        protected virtual void Complete()
        {
            if (string.IsNullOrEmpty(error))
            {
                onSuccess?.Invoke(this);
            }
            else
            {
                onFailure?.Invoke(this);
            }

            onComplete?.Invoke(this);
        }

        public RequestBase Send(Action done = null)
        {
            return SendBy(this.HttpMono(), done);
        }

        public RequestBase SendBy(MonoBehaviour sender, Action done = null)
        {
            return DoSend(sender, done);
        }

        public IEnumerator GetSendIEnumerator(Action done = null)
        {
            Prepare();
            www = CreateRequest();
            www.SetHeaders(header);
            www.timeout = timeout;
            return DoSendIE(www, done);
        }

        public abstract UnityWebRequest CreateRequest();

        private Coroutine _lastCoroutine;
        private MonoBehaviour _lastMono;

        protected RequestBase DoSend(MonoBehaviour sender, Action done = null)
        {
            Prepare();
            www = CreateRequest();
            www.SetHeaders(header);
            www.timeout = timeout;
            _lastMono = sender;
            _lastCoroutine = sender.StartCoroutine(DoSendIE(www, done));
            return this;
        }

        protected IEnumerator DoSendIE(UnityWebRequest www, Action done = null)
        {
            if (isUsingCache && cacher != null && cacher.TryGetCache(this, out string data))
            {
                isSuccess = true;
                text = data;
            }
            else
            {
#if UNITY_EDITOR
                var async = www.SendWebRequest();
                yield return new WaitUntil(() => async.isDone);
#else
                yield return  www.SendWebRequest();
#endif

                isSuccess = !www.isNetworkError && !www.isHttpError;
                error = www.error;
                text = www.downloadHandler?.text;

                if (isUsingCache && cacher != null)
                {
                    cacher.UpdateCache(hash, text, maxAge);
                }
            }

            Complete();
            done?.Invoke();
        }

        /// <summary>
        /// seconds
        /// </summary>
        /// <param name="maxAge"></param>
        /// <returns></returns>
        public RequestBase SetTimeout(int timeout)
        {
            this._timeout = timeout;
            return this;
        }

        /// <summary>
        /// seconds
        /// </summary>
        /// <param name="maxAge"></param>
        /// <returns></returns>
        public RequestBase SetMaxAge(long maxAge)
        {
            _maxAge = maxAge;
            return this;
        }

        public void Abort()
        {
            if (_lastMono)
            {
                _lastMono.StopCoroutine(_lastCoroutine);
                _lastMono = null;
                _lastCoroutine = null;
            }

            www?.Abort();
        }

        public void Dispose()
        {
            Abort();
            www?.Dispose();
        }

        public T GetResult<T>() where T : class
        {
            return result as T;
        }

        public T GetResult<T>(out T result) where T : class
        {
            result = this.result as T;
            return result;
        }


        public void Log(string text)
        {
            loger?.Log(text);
        }

        public void LogWarning(string text)
        {
            loger?.LogWarning(text);
        }

        public void LogError(string text)
        {
            loger?.LogError(text);
        }
    }
}