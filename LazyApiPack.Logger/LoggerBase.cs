using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace LazyApiPack.Logger
{
    public abstract class LoggerBase
    {
        protected List<string> _history = new();
        public Severity Verbosity { get; set; }
        public bool UseUtc { get; set; }
        [SupportedOSPlatform("windows")]
        public bool LogCriticalToEventlog { get; set; }

        private string _eventLogName;
        /// <summary>
        /// Sets the eventlog name
        /// </summary>
        /// <exception cref="">Is thrown if the application requires evlevated privilegues.</exception>
        public string EventLogName
        {
            get => _eventLogName; set
            {
                if (string.IsNullOrEmpty(value)) return;
                _eventLogName = value;
                if (!string.IsNullOrEmpty(EventLogSource))
                {
                    CreateEventLogSource();
                }
            }
        }

        private string _eventLogSource;
        /// <summary>
        /// Sets the eventlog source
        /// </summary>
        /// <exception cref="">Is thrown if the application requires evlevated privilegues.</exception>
        [SupportedOSPlatform("windows")]

        public string EventLogSource
        {
            get => _eventLogSource;
            set
            {
                if (string.IsNullOrEmpty(value)) return;
                _eventLogSource = value;
                if (!string.IsNullOrEmpty(EventLogName))
                {
                    CreateEventLogSource();
                }

            }
        }
        [SupportedOSPlatform("windows")]
        protected void CreateEventLogSource()
        {
            if (!EventLog.SourceExists(EventLogSource))
            {
                EventLog.CreateEventSource(new EventSourceCreationData(EventLogSource, EventLogName));
            }
        }
        private int _maxHistoryEntries = int.MaxValue;
        public int MaxHistoryEntries
        {
            get => _maxHistoryEntries;
            set
            {
                if (value < 0)
                {
                    value = 0;
                }

                if (value < _history.Count && _history.Count > 0)
                {
                    _history.RemoveRange(0, _history.Count - value);
                }
                _maxHistoryEntries = value;
            }
        }
        public IEnumerable<string> GetLogEntries(int takeLastEntries = int.MaxValue)
        {
            if (_history.Count > takeLastEntries)
            {
                return _history.Take(new Range(new Index(0, true), new Index(takeLastEntries, true)));
            }
            else
            {
                return _history;
            }
        }

    }
}
