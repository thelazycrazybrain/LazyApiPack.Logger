using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using LazyApiPack.Logger.Loggers;
using LazyApiPack.Logger.Tests.Helpers;

namespace LazyApiPack.Logger.Tests
{
    public class ConsoleLoggerTests
    {
        ConsoleLogger _consoleLogger;
        [SetUp]
        public void Setup()
        {
            _consoleLogger = new ConsoleLogger();
        }

        [Test]
        public void LogTests()
        {
            var warningMessage = "This is a warning";
            _consoleLogger.Log(Severity.Warning, warningMessage);

            var infoMessage = "This is a info";
            _consoleLogger.Log(Severity.Information, infoMessage);

            var verboseMessage = "This is verbose";
            _consoleLogger.Log(Severity.Verbose, verboseMessage);

            Assert.That(_consoleLogger.GetLogEntries().Count, Is.EqualTo(3));
            _consoleLogger.Verbosity = Severity.Information;

            _consoleLogger.Log(Severity.Verbose, verboseMessage);
            Assert.That(_consoleLogger.GetLogEntries().Count, Is.EqualTo(3));

            _consoleLogger.MaxHistoryEntries = 1;
            Assert.That(_consoleLogger.GetLogEntries().Count, Is.EqualTo(1));

            _consoleLogger.Log(Severity.Information, infoMessage);
            Assert.That(_consoleLogger.GetLogEntries().Count, Is.EqualTo(1));

            _consoleLogger.MaxHistoryEntries = int.MaxValue;
            
            
            if(RuntimeInformation.IsOSPlatform (OSPlatform.Windows))
            {
                if (PermissionManager.IsAdministrator)
                {
                    _consoleLogger.EventLogName = "LazyApiPack.Logger.Tests";
                    _consoleLogger.EventLogSource = $"LazyApiPack.Logger.Tests.{Guid.NewGuid()}";

                    var criticalMessage = "This is very critical";
                    _consoleLogger.LogCriticalToEventlog = true;
                    _consoleLogger.Log(Severity.Critical, criticalMessage);
                    Assert.True(EventLog.SourceExists(_consoleLogger.EventLogSource));
                    EventLog.DeleteEventSource(_consoleLogger.EventLogSource);
                } else
                {
                    Assert.Warn($"The event log test must be performed as an administrator.");
                }
                

            } else
            {
                Assert.Warn($"Runtime is {RuntimeInformation.RuntimeIdentifier} and not {nameof(OSPlatform.Windows)}.\r\nThe event log test can not be performed.");
            }
        }

       
    }
}