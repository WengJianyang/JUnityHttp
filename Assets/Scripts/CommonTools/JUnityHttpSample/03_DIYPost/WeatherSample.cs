using System.Collections.Generic;
using CommonTools.JUnityHttp.Cache;
using UnityEngine;

namespace CommonTools.JUnityHttpSample
{
    public class WeatherSample : MonoBehaviour
    {
        private void Start()
        {
            new WeatherPost<JWeatherInfo>(ApiUrl.WeatherInfo)
                .AddData(ApiKey.city, "Shanghai")
                .AddData(ApiKey.cityid, 24)
                .OnFailure(_ => { Debug.LogError(_.error); })
                .OnSuccess(_ =>
                {
                    _.GetResult(out JWeatherInfo info);
                    Debug.Log($"city={info.city},weather={info.weather},week={info.week},date={info.date}");
                })
                .Send();

            AppNetConfigSample.Init();
            HttpCacheDefault.Instance.LoadFormDisk(SendRequest);
        }

        public void SendRequest()
        {
            new WeatherPost<List<JCityInfo>>(ApiUrl.CityList)
                .OnFailure(_ => { _.LogError(_.error); })
                .OnSuccess(_ =>
                {
                    _.GetResult(out List<JCityInfo> info);
                    _.Log($"info count={info.Count}");
                })
                .UsingCache()
                .SetMaxAge(3600 * 24 * 7)
                .Send();

            new WeatherPost<JWeatherInfo>(ApiUrl.WeatherInfo)
                .AddData(ApiKey.city, "上海")
                .AddData(ApiKey.cityid, 24)
                .OnFailure(_ => { })
                .OnSuccess(_ =>
                {
                    _.GetResult(out JWeatherInfo info);
                    _.Log($"city={info.city},weather={info.weather},week={info.week},date={info.date}");
                })
                .UsingCache()
                .SetMaxAge(3600 * 24 * 7)
                .Send();
        }


        public void OnDestroy()
        {
            HttpCacheDefault.Instance.Save2Disk();
        }
    }
}