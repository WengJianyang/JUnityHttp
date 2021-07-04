using UnityEngine;

namespace CommonTools.JUnityHttp
{
    public class Parallel : MultiRequestBase
    {
        public Parallel(params RequestBase[] requestBases) : base(requestBases)
        {
        }

        public override MultiRequestBase SendBy(MonoBehaviour sender)
        {
            int reqCount = requests.Count;
            bool hadFailed = false;
            foreach (var requestBase in requests)
            {
                requestBase.SendBy(sender, () =>
                {
                    reqCount--;
                    //As long as one of the request failed, the other requests will abort
                    if (!requestBase.isSuccess && !hadFailed)
                    {
                        hadFailed = true;
                        Abort();
                        onFailure?.Invoke(requestBase.error);
                        onComplete?.Invoke();
                    }

                    if (reqCount <= 0 && !hadFailed)
                    {
                        onSuccess?.Invoke(this);
                        onComplete?.Invoke();
                    }
                });
            }

            return this;
        }
        
    }
}