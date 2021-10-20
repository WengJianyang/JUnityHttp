using System.Linq;
using UnityEngine.Networking;

namespace CommonTools.JUnityHttp
{
    public class Get : RequestBase
    {
        public Get(string url) : base(url)
        {
        }


        public override UnityWebRequest CreateRequest()
        {
            return UnityWebRequest.Get(url);
        }

        protected override void Prepare()
        {
            base.Prepare();
            if (questData!=null&&questData.Count>0)
            {
                url += $"?{string.Join("&", questData.Select(item => $"{item.Key}={item.Value}"))}";
            }
            loger?.Log($"send-get=={url}");
        }

        protected override void Complete()
        {
            loger?.Log($"recv-get==backMsg=={text}\nerrorMsg=={error}\nform url=={url}");
            base.Complete();
        }
    }
}