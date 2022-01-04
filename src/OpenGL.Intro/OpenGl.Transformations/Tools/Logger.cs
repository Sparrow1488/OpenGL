using System;

namespace OpenGl.Transformations.Tools
{
    internal class Logger
    {
        public void Log(string message)
        {
            Console.Write($"[{DateTime.Now}] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void Error(string message)
        {
            Console.Write($"[{DateTime.Now}] ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void Success(string message)
        {
            Console.Write($"[{DateTime.Now}] ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
