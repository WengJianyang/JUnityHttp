using UnityEngine;

namespace CommonTools.JUnityHttp.Sample.QuickStart
{
    public class QuickStart : MonoBehaviour
    {
        private void Start()
        {
            new Get(ApiUrl.BaiDuDoMain)
                .OnFailure(_ => Debug.LogError($"baidu error={_.error}"))
                .OnSuccess(_ => Debug.Log($"baidu back msg={_.text}"))
                .Send();

            new TaoBaoGet<TimeStepMiliSeconds>(ApiUrl.GetTaoBaoAPI3)
                .AddData(ApiKey.api, "mtop.common.getTimestamp")
                .OnFailure(_ => Debug.LogError($"taobao error={_.error}"))
                .OnSuccess(_ =>
                {
                    _.GetResult(out TimeStepMiliSeconds info);
                    Debug.Log($"TimeStepMiliSeconds={info.t}");
                })
                .Send();
        }
    }
}