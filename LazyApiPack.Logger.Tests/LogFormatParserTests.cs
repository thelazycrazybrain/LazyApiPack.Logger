using LazyApiPack.Logger.Tools;

namespace LazyApiPack.Logger.Tests
{
    public class LogFormatParserTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void ParseTest()
        {
            var dt = new DateTime(2023, 10, 31, 21, 55, 18);
            var msg = "This is a test message.";
            var verbose = LogFormatParser.Parse(LogFormats.VerboseLogFormat, LogFormats.VerboseCaption, msg, dt, LogFormats.FormatProvider);
            var information = LogFormatParser.Parse(LogFormats.InformationLogFormat, LogFormats.InformationCaption, msg, dt, LogFormats.FormatProvider);
            var warning = LogFormatParser.Parse(LogFormats.WarningLogFormat, LogFormats.WarningCaption, msg, dt, LogFormats.FormatProvider);
            var error = LogFormatParser.Parse(LogFormats.ErrorLogFormat, LogFormats.ErrorCaption, msg, dt, LogFormats.FormatProvider);
            var critical = LogFormatParser.Parse(LogFormats.CriticalLogFormat, LogFormats.CriticalCaption, msg, dt, LogFormats.FormatProvider);

            Assert.True(verbose == "2023/10/31 21:55:18 This is a test message.", $"Format for verbose is wrong \"{verbose}\"");
            Assert.True(information == "[Info] 2023/10/31 21:55:18 This is a test message.", $"Format for information is wrong \"{information}\"");
            Assert.True(warning == "[Warning] 2023/10/31 21:55:18 This is a test message.", $"Format for warning is wrong \"{warning}\"");
            Assert.True(error == "[Error] 2023/10/31 21:55:18 This is a test message.", $"Format for error is wrong \"{error}\"");
            Assert.True(critical == "[Critical] 2023/10/31 21:55:18 This is a test message.", $"Format for critical is wrong \"{critical}\"");

            Assert.Pass();
        }
    }
}