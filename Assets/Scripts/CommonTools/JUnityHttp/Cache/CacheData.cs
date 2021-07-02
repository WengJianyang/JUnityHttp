using System.Collections.Generic;

namespace CommonTools.JUnityHttp.Cache
{

    [System.Serializable]
    public class CacheData
    {
        /// <summary>
        /// hash  
        /// </summary>
        public string h;

        /// <summary>
        /// maxAge
        /// </summary>
        public long m;

        /// <summary>
        /// text
        /// </summary>
        public string t;


        public CacheData()
        {
        }

        public CacheData(string h, long m, string t)
        {
            this.h = h;
            this.m = m;
            this.t = t;
        }
    }

    [System.Serializable]
    public class VersionCacheData
    {
        public string version;
        public List<CacheData> datas;

        public VersionCacheData(string version, List<CacheData> datas)
        {
            this.version = version;
            this.datas = datas;
        }
    }
}