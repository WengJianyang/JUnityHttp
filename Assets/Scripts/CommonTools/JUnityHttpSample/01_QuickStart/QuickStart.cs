using CommonTools.JUnityHttp;
using UnityEngine;

namespace CommonTools.JUnityHttpSample
{
    public class QuickStart : MonoBehaviour
    {
        private void Start()
        {
            new Get("www.baidu.com")
                .OnFailure(_ => Debug.LogError($"get error={_.error}"))
                .OnSuccess(_ => Debug.Log($"baidu back msg={_.text}"))
                .Send();

            new Post("http://jisuqgtq.market.alicloudapi.com/weather/query")
                .SetHeader("Authorization", "APPCODE 70d20881c6e54725a5d2c63598c9cf64")
                .AddData(ApiKey.cityid, 24)
                .OnFailure(_ => Debug.LogError($"post error={_.error}"))
                .OnSuccess(_ =>
                {
                    _.GetResult(out JWeatherInfo info);
                    _.Log($"city={info.city},weather={info.weather},week={info.week},date={info.date}");
                })
                .Send();
        }
    }
}