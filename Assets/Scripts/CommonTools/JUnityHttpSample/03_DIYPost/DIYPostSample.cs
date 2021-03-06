using System;
using System.Collections.Generic;
using UnityEngine;

namespace CommonTools.JUnityHttpSample
{
    public class DIYPostSample : MonoBehaviour
    {
        private void Start()
        {
            
            new WeatherPost<List<JCityInfo>>(ApiUrl.CityList)
                .SetHeader("Authorization", "APPCODE 70d20881c6e54725a5d2c63598c9cf64")
                .OnFailure(_ => { Debug.LogError(_.error); })
                .OnSuccess(_ =>
                {
                    _.GetResult(out List<JCityInfo> info);
                    Debug.Log($"info count={info.Count}");
                })
                .Send();
            
            
            new WeatherPost<JWeatherInfo>(ApiUrl.WeatherInfo)
                .SetHeader("Authorization", "APPCODE 70d20881c6e54725a5d2c63598c9cf64")
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