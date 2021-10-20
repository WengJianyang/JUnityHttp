using UnityEngine.Networking;

namespace CommonTools.JUnityHttp
{
    public class Post : RequestBase
    {
        public Post(string url) : base(url)
        {
        }

        protected override void Prepare()
        {
            base.Prepare();
            loger?.Log($"send-post=={url}");
        }


        protected override void Complete()
        {
            loger?.Log($"recv-post==backMsg=={text}\nerrorMsg=={error}\nform url=={url}");
            base.Complete();
        }

        public override UnityWebRequest CreateRequest()
        {
            return UnityWebRequest.Post(url, questData);
        }
    }
}