using System;
using UnityEngine;

namespace CommonTools.JUnityHttpSample
{
    public class DIYPostSample : MonoBehaviour
    {
        private void Start()
        {
            AppNetConfigSample.Init();

            new WeatherPost<JWeatherInfo>(ApiUrl.WeatherInfo)
                .AddData(ApiKey.city, "上海")
                .AddData(ApiKey.cityid, 24)
                .OnFailure(_ => { Debug.LogError(_.error); })
                .OnSuccess(_ =>
                {
                    _.GetResult(out JWeatherInfo info);
                    Debug.Log($"city={info.city},weather={info.weather},week={info.week},date={info.date}");
                })
                .Send();
        }
    }
}