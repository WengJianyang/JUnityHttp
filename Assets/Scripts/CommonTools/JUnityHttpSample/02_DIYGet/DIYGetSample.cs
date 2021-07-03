using UnityEngine;

namespace CommonTools.JUnityHttpSample
{
    public class DIYGetSample : MonoBehaviour
    {
        private void Start()
        {
            new TaoBaoGet<TimeStampData>(ApiUrl.GetTaoBaoAPI3)
                .AddData(ApiKey.api, "mtop.common.getTimestamp")
                .OnFailure(_ => Debug.LogError($"taobao error={_.error}"))
                .OnSuccess(_ =>
                {
                    _.GetResult(out TimeStampData info);
                    Debug.Log($"current time stamp ={info.t}");
                })
                .Send();
        }
    }
}