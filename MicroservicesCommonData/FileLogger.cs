using System;
using System.IO;

namespace MicroservicesCommonData
{
    public class FileLogger
    {
        private string _logFileName;

        public FileLogger(string logFileName)
        {
            _logFileName = logFileName;
        }

        public void WriteMessage(string message) => File.AppendAllText(_logFileName, $"[{DateTime.Now.ToString("dd/MM/yyyy HH:mm")}]{Environment.NewLine}{message}{Environment.NewLine}");
    }
}