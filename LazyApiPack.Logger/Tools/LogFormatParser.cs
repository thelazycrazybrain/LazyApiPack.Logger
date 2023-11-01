using System.Globalization;
using System.Text.RegularExpressions;

namespace LazyApiPack.Logger.Tools
{
    public static class LogFormatParser
    {


        public static string Parse(string format, string severity, string message, DateTime timestamp, IFormatProvider formatProvider)
        {
            Regex _severityRegex = new Regex("\\{SEVERITY\\}", RegexOptions.Multiline);
            Regex _dateRegex = new Regex("\\{DATE.+?\\}", RegexOptions.Multiline);
            Regex _timeRegex = new Regex("\\{TIME.+?\\}", RegexOptions.Multiline);
            Regex _messageRegex = new Regex("\\{MESSAGE\\}", RegexOptions.Multiline);
            format = _severityRegex.Replace(format, severity);
            format = _messageRegex.Replace(format, message);
            var d = _dateRegex.Match(format);
            if (d.Success)
            {
                var value = d.Value.Substring(1, d.Value.Length - 2);
                if (value.ToUpper().StartsWith("DATE:"))
                {
                    var dFormat = value.Substring(5);
                    format = _dateRegex.Replace(format, timestamp.ToString(dFormat, formatProvider));
                }
                else
                {
                    format = _dateRegex.Replace(format, timestamp.ToString(formatProvider));
                }
            }
            var f = _timeRegex.Match(format);
            if (f.Success)
            {
                var value = f.Value.Substring(1, f.Value.Length - 2);
                if (value.ToUpper().StartsWith("TIME:"))
                {
                    var fFormat = value.Substring(5);
                    format = _timeRegex.Replace(format, timestamp.ToString(fFormat, formatProvider));
                }
                else
                {
                    format = _timeRegex.Replace(format, timestamp.ToString(formatProvider));
                }
            }

            return format;
        }
    }
}