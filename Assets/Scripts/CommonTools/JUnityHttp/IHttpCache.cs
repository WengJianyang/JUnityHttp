namespace CommonTools.JUnityHttp
{
    public interface IHttpCache
    {
        bool TryGetCache(RequestBase request, out string data);

        void UpdateCache(string hash, string text, long age);
    }
}