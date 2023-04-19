using System;
using MGNet;

namespace IOCPExampleServer
{
    class ServerStart
    {
        static void Main(string[] args)
        {
            IOCPServer server = new IOCPServer();
            server.StartAsServer("127.0.0.1", 17666, 100);
            Console.ReadKey();
        }
    }
}

