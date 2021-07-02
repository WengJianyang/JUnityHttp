namespace CommonTools.JUnityHttp.Sample
{
    public class ApiUrl
    {
        public static string BaiDuDoMain = "www.baidu.com";


        //taobao API
        public static string TaoBaoDoMain = "http://api.m.taobao.com";
        public static string GetTaoBaoAPI3 = TaoBaoDoMain + "/rest/api3.do?";

        //Weather API
        public const string WeatherDoMain = "http://jisuqgtq.market.alicloudapi.com";
        public const string WeatherInfo = WeatherDoMain + "/weather/query";
        public const string CityList = WeatherDoMain + "/weather/city";
    }
}