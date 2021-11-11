using System;
using CommonTools.JUnityHttp;
using UnityEngine;
using UnityEngine.Networking;

namespace CommonTools.JUnityHttpSample
{
    public class WeatherPost<T> : RequestBase where T : class
    {
        //Discuss with back-end engineer to use only one data format globally. (DRY) or you need to write more than one another response-class.
        private class Response<T1> where T1 : class
        {
            public string status { get; set; }
            public string msg { get; set; }
            public T1 result { get; set; }

            public bool IsSuccess()
            {
                return status == "0";
            }
        }

        public WeatherPost(string url) : base(url)
        {
        }

        protected override void Complete()
        {
            loger?.Log($"Recv-WeatherPost==backMsg=={text}\nerrorMsg=={error}\nform url=={url}");
            if (isSuccess)
            {
                Response<T> tmpData = null;
                try
                {
                    tmpData = text.ToObject<Response<T>>();
                }
                catch (Exception e)
                {
                    tmpData = null;
                    loger?.LogError(e.ToString());
                }
                finally
                {
                    if (tmpData != null)
                    {
                        if (tmpData.IsSuccess())
                        {
                            this.result = tmpData.result;
                            onSuccess?.Invoke(this);
                        }
                        else
                        {
                            isSuccess = false;
                            error = $"recv error status={tmpData.status} msg={tmpData.msg}";
                            onFailure?.Invoke(this);
                        }
                    }
                    else
                    {
                        isSuccess = false;
                        onFailure?.Invoke(this);
                    }
                }
            }
            else
            {
                string msg = !string.IsNullOrEmpty(error) ? error : text;
                if (isNetworkError)
                {
                    error = $"network exception, please check up your network";
                }
                else if (isHttpError)
                {
                    error = $"http error,you can try it again.error={msg}";
                }

                onFailure?.Invoke(this);
            }

            onComplete?.Invoke(this);
        }

        protected override void Prepare()
        {
            base.Prepare();
            loger?.Log($"Send-SamplePost={url}\nquest={questData.ToJson()}\n header={string.Join(",", header)}");
        }

        public override UnityWebRequest CreateRequest()
        {
            return UnityWebRequest.Post(url, questData);
        }
    }
}