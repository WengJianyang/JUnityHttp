using System;
using System.Collections.Generic;
using UnityEngine;

namespace CommonTools.JUnityHttp
{
    public abstract class MultiRequestBase : IDisposable
    {
        protected List<RequestBase> requests { get; private set; } = null;
        protected Action<MultiRequestBase> onSuccess;
        protected Action<string> onFailed;
        protected Action onComplete;

        public MultiRequestBase(params RequestBase[] requestBases)
        {
            if (requestBases != null && requestBases.Length > 0)
            {
                foreach (var request in requestBases)
                {
                    Append(request);
                }
            }
        }

        public MultiRequestBase Append(RequestBase request)
        {
            if (requests == null)
            {
                requests = new List<RequestBase>(2);
            }

            requests.Add(request);
            return this;
        }

        public MultiRequestBase OnSucceed(Action<MultiRequestBase> action)
        {
            onSuccess = action;
            return this;
        }

        public MultiRequestBase OnFailed(Action<string> action)
        {
            onFailed = action;
            return this;
        }

        public MultiRequestBase OnComplete(Action action)
        {
            onComplete = action;
            return this;
        }

        public abstract MultiRequestBase SendBy(MonoBehaviour sender);

        public MultiRequestBase Send()
        {
            return SendBy(this.HttpMono());
        }

        public void Abort()
        {
            foreach (var req in requests)
            {
                if (!req.www.isDone)
                {
                    req.Abort();
                }
            }
        }


        public void Dispose()
        {
            foreach (var req in requests)
            {
                req.Dispose();
            }
        }
    }
}