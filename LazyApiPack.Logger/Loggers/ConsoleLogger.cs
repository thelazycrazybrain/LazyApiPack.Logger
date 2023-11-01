using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using LazyApiPack.Logger.Tools;

namespace LazyApiPack.Logger.Loggers
{
    public class ConsoleLogger : LoggerBase, ILogger
    {

        public void Flush()
        {
        }

        public void Log(Severity severity, string message)
        {
            if ((int)Verbosity > (int)severity) return;
            if (_history.Count >= MaxHistoryEntries && MaxHistoryEntries > 0)
            {
                _history.Remove(_history.First());
            }

            string logEntry = null;
            ConsoleColor color = Console.ForegroundColor;
            switch (severity)
            {
                case Severity.Information:
                    logEntry = LogFormatParser.Parse(InformationLogFormat.LogFormat, InformationLogFormat.SeverityCaption,
                        message, UseUtc ? DateTime.UtcNow : DateTime.Now, InformationLogFormat.FormatProvider);
                    color = InformationLogFormat.Color;
                    break;
                case Severity.Warning:
                    logEntry = LogFormatParser.Parse(WarningLogFormat.LogFormat, WarningLogFormat.SeverityCaption,
                        message, UseUtc ? DateTime.UtcNow : DateTime.Now, WarningLogFormat.FormatProvider);
                    color = WarningLogFormat.Color;
                    break;
                case Severity.Error:
                    logEntry = LogFormatParser.Parse(ErrorLogFormat.LogFormat, ErrorLogFormat.SeverityCaption,
                        message, UseUtc ? DateTime.UtcNow : DateTime.Now, ErrorLogFormat.FormatProvider);
                    color = ErrorLogFormat.Color;
                    break;
                case Severity.Critical:
                    logEntry = LogFormatParser.Parse(CriticalLogFormat.LogFormat, CriticalLogFormat.SeverityCaption,
                        message, UseUtc ? DateTime.UtcNow : DateTime.Now, CriticalLogFormat.FormatProvider);
                    color = CriticalLogFormat.Color;
                    break;
                default:
                    logEntry = LogFormatParser.Parse(VerboseLogFormat.LogFormat, VerboseLogFormat.SeverityCaption,
                        message, UseUtc ? DateTime.UtcNow : DateTime.Now, VerboseLogFormat.FormatProvider);
                    color = VerboseLogFormat.Color;
                    break;
            }

            _history.Add(logEntry);

            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(logEntry);
            Console.ForegroundColor = oldColor;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (LogCriticalToEventlog && severity == Severity.Critical)
                {
                    try
                    {
                        var log = new EventLog();
                        log.Source = EventLogSource;
                        log.WriteEntry(logEntry, EventLogEntryType.Error);
                    }
                    catch (Exception ex)
                    {
                        Log(Severity.Error, $"The error can not be written to the event log due to an error.\r\n{ex.ToString()}");

                    }

                }
            }
        }

        public ConsoleLogFormat VerboseLogFormat { get; set; }
            = new ConsoleLogFormat(ConsoleColor.White, LogFormats.VerboseLogFormat, LogFormats.VerboseCaption, LogFormats.FormatProvider);

        public ConsoleLogFormat InformationLogFormat { get; set; }
            = new ConsoleLogFormat(ConsoleColor.Green, LogFormats.InformationLogFormat, LogFormats.InformationCaption, LogFormats.FormatProvider);

        public ConsoleLogFormat WarningLogFormat { get; set; }
            = new ConsoleLogFormat(ConsoleColor.Yellow, LogFormats.WarningLogFormat, LogFormats.WarningCaption, LogFormats.FormatProvider);

        public ConsoleLogFormat ErrorLogFormat { get; set; }
            = new ConsoleLogFormat(ConsoleColor.Red, LogFormats.ErrorLogFormat, LogFormats.ErrorCaption, LogFormats.FormatProvider);

        public ConsoleLogFormat CriticalLogFormat { get; set; }
            = new ConsoleLogFormat(ConsoleColor.Red, LogFormats.CriticalLogFormat, LogFormats.CriticalCaption, LogFormats.FormatProvider);

    }

    public struct ConsoleLogFormat
    {
        public ConsoleLogFormat(ConsoleColor color, string logFormat, string severityCaption, IFormatProvider formatProvider)
        {
            Color = color;
            LogFormat = logFormat;
            SeverityCaption = severityCaption;
            FormatProvider = formatProvider;
        }
        public ConsoleColor Color { get; set; }
        public string LogFormat { get; set; }
        public string SeverityCaption { get; set; }
        public IFormatProvider FormatProvider { get; set; }
    }
}