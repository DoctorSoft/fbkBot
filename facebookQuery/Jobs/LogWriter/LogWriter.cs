using System;
using System.IO;

namespace Jobs.LogWriter
{
    public static class LogWriter
    {
        const string LogFile = "log.txt";

        public static void AddToLog(string text)
        {
            try
            {
                var resultText = string.Format("\r\n[{0}] {1}", DateTime.Now, text);

                if (!File.Exists(LogFile))
                {
                    return;
                }

                File.AppendAllText(LogFile, resultText);
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
