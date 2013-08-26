using System;

namespace Blitz.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Service starting...");

            var serverBootstrapper = new Bootstrapper();

            Console.ReadKey(false);

            serverBootstrapper.Host.Close();
        }
    }
}