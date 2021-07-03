using System;
using Newtonsoft.Json;

namespace CommonTools.JUnityHttpSample
{
    public static class JsonExtensions 
    {

        public static T ToObject<T>(this string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
                throw new Exception($"Deserialize JsonString to Object failure, json={json} error={e.Message}");
            }
        }

        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
