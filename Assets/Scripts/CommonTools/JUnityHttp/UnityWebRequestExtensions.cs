using System;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace CommonTools.JUnityHttp
{
    public static class UnityWebRequestExtensions
    {
        public static void SetHeaders(this UnityWebRequest self, Dictionary<string, string> header)
        {
            if (header != null && header.Count > 0)
            {
                foreach (var head in header)
                {
                    self.SetRequestHeader(head.Key, Convert.ToString(head.Value));
                }
            }
        }
    }
}