using System;
using CommonTools.JUnityHttp;
using UnityEngine;

namespace CommonTools.JUnityHttpSample
{
    public class ResponseCacheSample : MonoBehaviour
    {
        private void Start()
        {
            //setup global config 
            AppNetConfigSample.Init();
            
            new Get("www.baidu.com")
                .OnFailure(_ => Debug.LogError($"get error={_.error}"))
                .OnSuccess(_ => Debug.Log($"baidu back msg={_.text}"))
                .UsingCache()
                .SetMaxAge(3600*24)//Valid for one day
                .Send();
        }
    }
}