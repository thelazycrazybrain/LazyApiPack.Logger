# About this pack
This library provides logging functionality for applications

# General usage
To use the classes, instantiate the logger as a new instance or use dependency injection with the `ILogger` interface.

## Log event
To log an event, use the `ILogger.Log` method.
- Verbosity: Sets the log level from where logs are stored
- UseUtc: Logs the entries either with the machine local time or the UTC time
- LogCriticalToEventLog (Windows only): If the event was logged critically, the event will also be stored in the Windows event log. This requires EventLogName and EventLogSource to be not null. Also, if the source / name don't exist, the creation requires administrator privilegues. Please ensure to set those up in your application setup if the application is not supposed to run as administrator.
- EventLogName (Windows only): The name of the event log entries.
- EventLogSource (Windows only): The source of the event log entries.
- MaxHistoryEntries: This entry specifies, how many logs are stored in the application to be used by the "GetLogEntries" method.


# Console Logger
The console logger can be configured via the VerboseLogFormat, InformationLogFormat etc.
You can specify the log format, time format and culture info. Also, you can specify the color in which the console text is displayed (Not thread safe!)


# File Logger
To set up the file logger, you must specify a file name.
You can set the AutoFlush method to true, if you want to automatically push every log directly to the file.

## Initialization

```csharp
var fileLogger = new FileLogger();
fileLogger.File = Path.Combine(Path.GetTempPath(), $"LazyApiPack.Logger.Tests.{Guid.NewGuid()}.log");
fileLogger.AutoFlush = true;
```

## Dispose
Either you flush the log after the application closes, or you use the GC to dispose this class
```csharp
_fileLogger.Dispose();
```

# Log Formats
You can create a format in which the log entries are stored.

## Placeholders
You can use placeholders and respective formats for certain parts of the log entry.
```
[{SEVERITY}] {DATE:yyyy/MM/dd} {TIME:HH:mm:ss} {MESSAGE}
```

### {SEVERITY}
Places the severity text into the string.

## {DATE}
Places the date of the log entry into the string
```
{DATE}
```
This logs the date in the format of the specified culture info.

```
{DATE:yyyy/MM/dd}
```
This logs the date in the specific format yyyy/MM/dd.

## {TIME}
Places the time of the log entry into the string
```
{TIME}
```
This logs the time in the format of the specified culture info.

```
{TIME:HH:mm:ss}
```
This logs the time in the specific format HH:mm:ss.

## {MESSAGE}
Places the message into the string.
This placeholder does not support a format specifier.
```
{MESSAGE}
```
## Using your own logger functionality
To use a custom logger, use the `ILogger` interface.
To parse the format, use the `LogFormatParser.Parse` method.
You find default format strings in the class `LogFormats`

## Format specifiers
The format specifier uses the default c# `DateTime.ToString()` format specifier.
The formatProvider is used by the respective configuration.
