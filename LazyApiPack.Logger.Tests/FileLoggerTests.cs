using LazyApiPack.Logger.Loggers;
using LazyApiPack.Logger.Tests.Helpers;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace LazyApiPack.Logger.Tests
{
    public class FileLoggerTests
    {
        FileLogger _fileLogger;

        [SetUp]
        public void Setup()
        {
            _fileLogger = new FileLogger();
            _fileLogger.File = Path.Combine(Path.GetTempPath(), $"LazyApiPack.Logger.Tests.{Guid.NewGuid()}.log");
            _fileLogger.AutoFlush = true;
        }

        [Test]
        public void LogTests()
        {
            var warningMessage = "This is a warning";
            _fileLogger.Log(Severity.Warning, warningMessage);

            var infoMessage = "This is a info";
            _fileLogger.Log(Severity.Information, infoMessage);

            var verboseMessage = "This is verbose";
            _fileLogger.Log(Severity.Verbose, verboseMessage);

            Assert.That(_fileLogger.GetLogEntries().Count, Is.EqualTo(3));
            _fileLogger.Verbosity = Severity.Information;

            _fileLogger.Log(Severity.Verbose, verboseMessage);
            Assert.That(_fileLogger.GetLogEntries().Count, Is.EqualTo(3));

            _fileLogger.MaxHistoryEntries = 1;
            Assert.That(_fileLogger.GetLogEntries().Count, Is.EqualTo(1));

            _fileLogger.Log(Severity.Information, infoMessage);
            Assert.That(_fileLogger.GetLogEntries().Count, Is.EqualTo(1));

            _fileLogger.MaxHistoryEntries = int.MaxValue;


            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (PermissionManager.IsAdministrator)
                {
                    _fileLogger.EventLogName = "LazyApiPack.Logger.Tests";
                    _fileLogger.EventLogSource = $"LazyApiPack.Logger.Tests.{Guid.NewGuid()}";

                    var criticalMessage = "This is very critical";
                    _fileLogger.LogCriticalToEventlog = true;
                    _fileLogger.Log(Severity.Critical, criticalMessage);
                    Assert.True(EventLog.SourceExists(_fileLogger.EventLogSource));
                    EventLog.DeleteEventSource(_fileLogger.EventLogSource);
                }
                else
                {
                    Assert.Warn($"The event log test must be performed as an administrator.");
                }


            }
            else
            {
                Assert.Warn($"Runtime is {RuntimeInformation.RuntimeIdentifier} and not {nameof(OSPlatform.Windows)}.\r\nThe event log test can not be performed.");
            }
        }

        [TearDown]
        public void TearDown()
        {
            var file = _fileLogger.File;
            _fileLogger.Dispose();
            File.Delete(file);
        }
    }
}