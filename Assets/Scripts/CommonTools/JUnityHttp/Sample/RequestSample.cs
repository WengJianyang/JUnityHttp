using System;
using System.Collections;
using UnityEngine;

namespace CommonTools.JUnityHttp.Sample
{
    public class RequestSample : MonoBehaviour
    {
        private void Start()
        {
            AppNetConfigSample.Init();

        }


        public Coroutine Delay(float timer, Action act)
        {
            return StartCoroutine(DelayIE(timer, act));
        }

        public IEnumerator DelayIE(float timer, Action act)
        {
            yield return new WaitForSeconds(timer);
            act?.Invoke();
        }
    }
}