using EShoppy.PomocniModul.Interfejsi;
using System;

namespace EShoppy.PomocniModul.Implementacija
{
    public class ConsoleLogger : ILogger
    {
        public void LogMessage(string Message)
        {
            Console.WriteLine($"{DateTime.Now} - {Message}");
        }
    }
}
