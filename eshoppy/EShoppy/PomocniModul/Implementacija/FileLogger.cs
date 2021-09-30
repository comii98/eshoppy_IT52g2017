using EShoppy.PomocniModul.Interfejsi;
using System;
using System.IO;

namespace EShoppy.PomocniModul.Implementacija
{
    public class FileLogger : ILogger
    {
        public void LogMessage(string Message)
        {
            using (StreamWriter w = File.AppendText("log.txt"))
            {
                w.WriteLine($"{DateTime.Now} - {Message}");
            }
        }
    }
}
