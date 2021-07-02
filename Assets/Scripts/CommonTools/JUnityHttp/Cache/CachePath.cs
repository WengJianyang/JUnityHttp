using UnityEngine;

namespace CommonTools.JUnityHttp.Cache
{
    /// <summary>
    /// Provides a path for reading and writing
    /// </summary>
    public class CachePath
    {
        public static string CanReadAndWritePath(bool isRead)
        {
            string path = Application.persistentDataPath;

            if (isRead)
            {
                return GetPreFilePrefix(path);
            }

            return path;
        }

        public static string GetPreFilePrefix(string path)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    return path;
                default:
                    return "file://" + path;
            }
        }
    }
}