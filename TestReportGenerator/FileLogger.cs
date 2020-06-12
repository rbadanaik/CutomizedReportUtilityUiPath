using System.IO;

namespace TestReportGenerator
{
    public enum LogTarget
    {
        File, EventLog
    }
    class FileLogger : LogBase
    {
        public void Log(string message)
        {
            string curDir = Directory.GetCurrentDirectory();
            string logDirectory = $@"{curDir}\executionlogs";

            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
            string filePath = $@"{logDirectory}\logfile.txt";

            if (!File.Exists(filePath))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.WriteLine(message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine(message);
                }
            }
        }
    }
}