using System.Globalization;

namespace LazyApiPack.Logger
{
    public static class LogFormats
    {
        public static string VerboseLogFormat = "{DATE:yyyy/MM/dd} {TIME:HH:mm:ss} {MESSAGE}";
        public static string VerboseCaption = "Verbose";
        public static IFormatProvider FormatProvider = CultureInfo.InvariantCulture;
        public static string InformationLogFormat = "[{SEVERITY}] {DATE:yyyy/MM/dd} {TIME:HH:mm:ss} {MESSAGE}";
        public static string InformationCaption = "Info";
        public static string WarningLogFormat = "[{SEVERITY}] {DATE:yyyy/MM/dd} {TIME:HH:mm:ss} {MESSAGE}";
        public static string WarningCaption = "Warning";
        public static string ErrorLogFormat = "[{SEVERITY}] {DATE:yyyy/MM/dd} {TIME:HH:mm:ss} {MESSAGE}";
        public static string ErrorCaption = "Error";
        public static string CriticalLogFormat = "[{SEVERITY}] {DATE:yyyy/MM/dd} {TIME:HH:mm:ss} {MESSAGE}";
        public static string CriticalCaption = "Critical";
    }
}