using System;

using Blitz.Common.Core;

namespace Blitz.Server.Core
{
    public class ConsoleLogger : ILog
    {
        public void Warn(string format, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(CreateLogMessage(format, args), "WARN");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void Error(Exception exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(CreateLogMessage(exception.ToString()), "ERROR");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void Info(string format, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(CreateLogMessage(format, args), "INFO");
        }

        private static string CreateLogMessage(string format, params object[] args)
        {
            return string.Format("[{0}] {1}",
                DateTime.Now.ToString("o"),
                string.Format(format, args));
        }
    }
}