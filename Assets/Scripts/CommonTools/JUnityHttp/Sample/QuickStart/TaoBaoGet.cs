using System;
using System.Collections.Generic;

namespace CommonTools.JUnityHttp.Sample.QuickStart
{
    public class TaoBaoGet<T> : Get
    {
        public TaoBaoGet(string url) : base(url)
        {
        }

        public class Response<T1>
        {
            public string api { get; set; }
            public string v { get; set; }
            public List<string> ret { get; set; }
            public T1 data { get; set; }

            public bool IsSuccess()
            {
                return Status().Contains("SUCCESS");
            }

            public string Status()
            {
                if (ret != null && ret.Count > 0)
                {
                    return ret[0];
                }

                return "";
            }
        }


        protected override void Complete()
        {
            Log($"Recv-WeatherPost==backMsg=={text}\nerrorMsg=={error}\nform url=={url}");

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
                    LogError(e.ToString());
                }
                finally
                {
                    if (tmpData != null)
                    {
                        if (tmpData.IsSuccess())
                        {
                            this.result = tmpData.data;
                            onSuccess?.Invoke(this);
                        }
                        else
                        {
                            isSuccess = false;
                            error = $"recv error status={tmpData.Status()}";
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
                if (www.isNetworkError)
                {
                    error = $"network exception, please reset your network";
                }
                else if (www.isHttpError)
                {
                    error = $"http error,you can try it again.error={www.error}";
                }

                onFailure?.Invoke(this);
            }

            onComplete?.Invoke(this);
        }
    }
}