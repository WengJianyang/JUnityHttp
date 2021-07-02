namespace CommonTools.JUnityHttp
{
    public interface IHttpLog
    {
        void Log(string text);

        void LogWarning(string text);

        void LogError(string text);
    }
}