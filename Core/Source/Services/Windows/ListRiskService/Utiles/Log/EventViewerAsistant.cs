using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Utiles.Log
{
    public class EventViewerAsistant
    {
        private static object ob = new object();

        private static EventViewerAsistant _SingleInstance;

        public static EventViewerAsistant SingleInstance
        {
            get
            {
                if (_SingleInstance == null)
                {
                    lock (ob)
                    {
                        if (_SingleInstance == null)
                        {
                            _SingleInstance = new EventViewerAsistant();
                        }
                    }
                }
                return _SingleInstance;
            }
        }

        public void WriteInEventViewer(
            string messageLog,
            EventLogEntryType eventType = EventLogEntryType.Information,
            int messageCode = 200)
        {
            var source = Resources.ServiceName;
            var log = Resources.ServiceName;

            if (!EventLog.SourceExists(source))
            {
                EventLog.CreateEventSource(source, log);
            }

            EventLog.WriteEntry(source, messageLog, eventType, messageCode);
        }
    }
}
