using System;

namespace MicroWebServer.WebServer.Logging
{
    public class ConsoleLog : ILog
    {
        public void Alert(string Message)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"(A) {DateTime.Now} | {Message}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void Critical(string Message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"(C) {DateTime.Now} | {Message}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void Debug(string Message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"(DEBUG) {DateTime.Now} | {Message}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void Error(string Message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"(E) {DateTime.Now} | {Message}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void Informational(string Message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"(I) {DateTime.Now} | {Message}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void Warning(string Message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"(W) {DateTime.Now} | {Message}");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
