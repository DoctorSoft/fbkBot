using System;
using System.IO;

namespace Services.LogWriter
{
    public static class LogWriter
    {
        const string LogFile = "log.txt";

        public static void AddToLog(string text)
        {
            var resultText = string.Format("\r\n[{0}] {1}", DateTime.Now, text);

            if (!File.Exists(LogFile))
            {
                return;
            }

            File.AppendAllText(LogFile, resultText);
        }
    }
}
