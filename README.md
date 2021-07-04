# JUnityHttp
[中文文档](README_CN.md): quick start.
I am a more convenient way for making http requests in unity base on UnityWebRequest.
If UnityWebRequest works well on your project, so does JUnityHttp.

## Features

* Reactive request
* Batch requests, and call back when all are completed
* Response can be cached by time and version

## Install

Copy ~Assets/Scripts/CommonTools/JUnityHttp to your project

## QuickStart

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

## More example

* [01_QuickStart](Assets/Scripts/CommonTools/JUnityHttpSample/01_QuickStart/): quick start.
* [02_DIYGet](Assets/Scripts/CommonTools/JUnityHttpSample/02_DIYGet/):diy your get.
* [03_DIYPost](Assets/Scripts/CommonTools/JUnityHttpSample/03_DIYPost/): diy your post.
* [04_ParallelRequest](Assets/Scripts/CommonTools/JUnityHttpSample/04_ParallelRequest/): Batch requests, and call back when all are completed.
* [05_ResponseCache](Assets/Scripts/CommonTools/JUnityHttpSample/05_ResponseCache/): response cache.
