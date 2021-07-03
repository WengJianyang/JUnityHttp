using System;
using CommonTools.JUnityHttp;
using UnityEngine;

namespace CommonTools.JUnityHttpSample
{
    public class ParallelSample : MonoBehaviour
    {
        private void Start()
        {
            AppNetConfigSample.Init();

            var request1 = new Get(ApiUrl.BaiDuDoMain);

            var request2 = new TaoBaoGet<TimeStampData>(ApiUrl.GetTaoBaoAPI3)
                .AddData(ApiKey.api, "mtop.common.getTimestamp");

            var request3 = new WeatherPost<JWeatherInfo>(ApiUrl.WeatherInfo)
                .AddData(ApiKey.city, "上海")
                .AddData(ApiKey.cityid, 24);

            new Parallel(request1, request2, request3)
                .OnSucceed(p =>
                {
                    Debug.Log($"baidu back msg={request1.text}");
                    
                    request2.GetResult(out TimeStampData infoTime);
                    Debug.Log($"current time stamp ={infoTime.t}");
                    
                    request3.GetResult(out JWeatherInfo info);
                    Debug.Log($"city={info.city},weather={info.weather},week={info.week},date={info.date}");
                })
                .OnFailed(p => { Debug.Log($"并行请求失败{p}"); })
                .SendBy(this);
        }
    }
}