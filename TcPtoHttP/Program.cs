using System;

namespace TcPtoHttP
{
    class Program
    {
        static void Main(string[] args)
        {
            // Pass one of these two in line 14. OR make TcpServer to accept a list of actions.
            Action<string> printToConsoleAction = (msg) => Console.WriteLine(msg);
            Action<string> sendHttpMessage = (msg) =>  new HttpAction("http://www.someapi.dz/api/v1").Post(msg).Wait();

            TcpServer server = new TcpServer();
            server.Start(666, printToConsoleAction);

            Console.ReadLine();
            server.Stop();
        }
    }
}
