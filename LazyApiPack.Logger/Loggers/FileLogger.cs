using LazyApiPack.Logger.Tools;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

namespace LazyApiPack.Logger.Loggers
{
    public class FileLogger : LoggerBase, ILogger, IDisposable
    {
        private FileStream _fileStream;
        public FileLogger()
        {

        }
        public void Flush()
        {
            if (_fileStream != null)
            {
                _fileStream.Flush();
            }
        }



        public void Log(Severity severity, string message)
        {
            if (_fileStream == null) throw new ArgumentNullException("Logfile is currently not open.");
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

                    break;
                case Severity.Warning:
                    logEntry = LogFormatParser.Parse(WarningLogFormat.LogFormat, WarningLogFormat.SeverityCaption,
                        message, UseUtc ? DateTime.UtcNow : DateTime.Now, WarningLogFormat.FormatProvider);

                    break;
                case Severity.Error:
                    logEntry = LogFormatParser.Parse(ErrorLogFormat.LogFormat, ErrorLogFormat.SeverityCaption,
                        message, UseUtc ? DateTime.UtcNow : DateTime.Now, ErrorLogFormat.FormatProvider);

                    break;
                case Severity.Critical:
                    logEntry = LogFormatParser.Parse(CriticalLogFormat.LogFormat, CriticalLogFormat.SeverityCaption,
                        message, UseUtc ? DateTime.UtcNow : DateTime.Now, CriticalLogFormat.FormatProvider);

                    break;
                default:
                    logEntry = LogFormatParser.Parse(VerboseLogFormat.LogFormat, VerboseLogFormat.SeverityCaption,
                        message, UseUtc ? DateTime.UtcNow : DateTime.Now, VerboseLogFormat.FormatProvider);

                    break;
            }

            _history.Add(logEntry);

            var enc = new UTF8Encoding();
            var data = enc.GetBytes(logEntry + "\r\n");
            _fileStream.Write(data);
            if (AutoFlush)
            {
                Flush();
            }
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

        public FileLogFormat VerboseLogFormat { get; set; }
            = new FileLogFormat(LogFormats.VerboseLogFormat, LogFormats.VerboseCaption, LogFormats.FormatProvider);

        public FileLogFormat InformationLogFormat { get; set; }
            = new FileLogFormat(LogFormats.InformationLogFormat, LogFormats.InformationCaption, LogFormats.FormatProvider);

        public FileLogFormat WarningLogFormat { get; set; }
            = new FileLogFormat(LogFormats.WarningLogFormat, LogFormats.WarningCaption, LogFormats.FormatProvider);

        public FileLogFormat ErrorLogFormat { get; set; }
            = new FileLogFormat(LogFormats.ErrorLogFormat, LogFormats.ErrorCaption, LogFormats.FormatProvider);

        public FileLogFormat CriticalLogFormat { get; set; }
            = new FileLogFormat(LogFormats.CriticalLogFormat, LogFormats.CriticalCaption, LogFormats.FormatProvider);

        private string _file;
        public string File
        {
            get => _file; set
            {
                if (string.IsNullOrEmpty(value)) return;
                _file = value;
                if (_fileStream != null)
                {
                    Flush();
                    _fileStream.Close();
                }
                
                _fileStream = System.IO.File.Open(_file, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);

                _history.Clear();
            }
        }

        private bool _autoFlush = true;
        public bool AutoFlush
        {
            get => _autoFlush; set
            {
                _autoFlush = value;
                if (_fileStream != null && value)
                {
                    _fileStream.Flush();
                }

            }
        }

        public bool IsDisposed { get; protected set; }
        public void Dispose()
        {
            if (!IsDisposed)
            {
                Dispose(false);
                GC.SuppressFinalize(this);
                IsDisposed = true;

            }

        }
        ~FileLogger()
        {
            Dispose(true);

        }

        protected void Dispose(bool disposing)
        {
            Flush();

            if (_fileStream != null)
            {
                _fileStream.Close();
            }
            _file = null;
            // if disposing is true, the CLR-GC is in progress
            // TODO: Add logic to dispose the class properly

        }
    }

    public struct FileLogFormat
    {
        public FileLogFormat(string logFormat, string severityCaption, IFormatProvider formatProvider)
        {
            LogFormat = logFormat;
            SeverityCaption = severityCaption;
            FormatProvider = formatProvider;
        }
        public string LogFormat { get; set; }
        public string SeverityCaption { get; set; }
        public IFormatProvider FormatProvider { get; set; }
    }

}