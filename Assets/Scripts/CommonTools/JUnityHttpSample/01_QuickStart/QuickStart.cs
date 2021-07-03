using CommonTools.JUnityHttp;
using UnityEngine;

namespace CommonTools.JUnityHttpSample
{
    public class QuickStart : MonoBehaviour
    {
        private void Start()
        {
            new Get(ApiUrl.BaiDuDoMain)
                .OnFailure(_ => Debug.LogError($"baidu error={_.error}"))
                .OnSuccess(_ => Debug.Log($"baidu back msg={_.text}"))
                .Send();
        }
    }
}