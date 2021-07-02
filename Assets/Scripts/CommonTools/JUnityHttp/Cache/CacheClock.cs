using System;

namespace CommonTools.JUnityHttp.Cache
{
    public static class CacheClock
    {
        public static long current => ToTimeStampSeconds(DateTime.Now);


        public static bool IsCacheTimeOut(this long time)
        {
            return time < current;
        }


        /// <summary>
        /// Converting DateTime to time stamp  in seconds
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private static long ToTimeStampSeconds(DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            return (time.Ticks - startTime.Ticks) / 10000000;
        }
    }
}