using UnityEngine;

namespace CommonTools.JUnityHttp
{
    public class HttpMono : MonoBehaviour
    {
        private static HttpMono _instance;

        private static bool forceDestory = false;

        public static HttpMono Instance
        {
            get
            {
                if (_instance == null)
                {
                    if (!forceDestory)
                    {
                        var g = new GameObject("JUnityHttpMono");
                        DontDestroyOnLoad(g);
                        _instance = g.AddComponent<HttpMono>();
                    }
                }

                return _instance;
            }
        }


        public static void ForceDestroy()
        {
            if (_instance)
            {
                Destroy(_instance.gameObject);
                _instance = null;
                forceDestory = true;
            }
        }
    }

    public static class HttpMonoExtensions
    {
        public static MonoBehaviour HttpMono(this System.Object self)
        {
            return JUnityHttp.HttpMono.Instance;
        }
    }
}