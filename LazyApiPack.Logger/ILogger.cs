using System.Runtime.Versioning;

namespace LazyApiPack.Logger
{
    public interface ILogger
    {
        [UnsupportedOSPlatform("ios")]
        [UnsupportedOSPlatform("tvos")]
        [SupportedOSPlatform("maccatalyst")]
        bool LogCriticalToEventlog { get; set; }
        Severity Verbosity { get; set; }
        bool UseUtc { get; set; }
        void Log(Severity severity, string message);
        IEnumerable<string> GetLogEntries(int takeLastEntries);
        
            void Flush();
    }
}