using System.Collections.Generic;

namespace CommonTools.JUnityHttp
{
    public interface IHttpHeader
    {
        Dictionary<string, string> GetByHeader(string url);
    }
}