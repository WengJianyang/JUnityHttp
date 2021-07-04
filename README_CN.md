# JUnityHttp
* 一个更省事的方式在unity里边去发送http请求，基于UnityWebRequest。
* 如果UnityWebRequest能完成你项目的功能，那么JUnityHttp也行。

## 功能点

* 响应式请求
* 支持多个请求同时处理，并统一回调
* 按时间跟版本号进行缓存

## 安装

Copy ~Assets/Scripts/CommonTools/JUnityHttp to your project

## 快速开始

```C#
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
        Debug.Log($"city={info.city},weather={info.weather},week={info.week},date={info.date}");
    })
    .Send();
```

## 更多范例

* [01_QuickStart](Assets/Scripts/CommonTools/JUnityHttpSample/01_QuickStart/QuickStart.cs):快速开始。
* [02_DIYGet](Assets/Scripts/CommonTools/JUnityHttpSample/02_DIYGet/):自定义get。
* [03_DIYPost](Assets/Scripts/CommonTools/JUnityHttpSample/03_DIYPost/): 自定义post。
* [04_ParallelRequest](Assets/Scripts/CommonTools/JUnityHttpSample/04_ParallelRequest/ParallelRequestSample.cs): 批量请求，并统一回调。
* [05_ResponseCache](Assets/Scripts/CommonTools/JUnityHttpSample/05_ResponseCache/ResponseCacheSample.cs): 请求缓存。
