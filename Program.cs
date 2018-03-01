using System;

namespace exnet
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create Connection ...");
            ExWSClient client = new ExWSClient();
            
            Console.ReadKey();
        }
    }
}
