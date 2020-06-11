using System.IO;

namespace TestReportGenerator
{
    public enum LogTarget
    {
        File, Database, EventLog
    }
    class FileLogger : LogBase
    {

        string curDir = Directory.GetCurrentDirectory();
        public string filePath = @"C:\Users\ranbadan\Documents\UiPath\TestReportAndLogger\logfile.txt";
        public void Log(string message)
        {
            using (StreamWriter streamWriter = new StreamWriter(filePath))
            {
                streamWriter.WriteLine(message);
                streamWriter.Close();
            }
        }
    }
}