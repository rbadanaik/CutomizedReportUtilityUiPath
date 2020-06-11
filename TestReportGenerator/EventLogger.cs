using System.Diagnostics;
using System.IO;

namespace TestReportGenerator
{
    class EventLogger : LogBase
    {
        public void Log(string message)
        {
            EventLog eventLog = new EventLog();
            eventLog.Source = "IDGEventLog";
            eventLog.WriteEntry(message);
        }
    }
}
