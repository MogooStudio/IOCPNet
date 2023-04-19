using System;
using MGNet;

namespace IOCPExampleClient
{
    class ClientStart
    {
        static void Main(string[] args)
        {
            IOCPClient client = new IOCPClient();
            client.StartAsClient("127.0.0.1", 17666);
            Console.ReadKey();
        }
    }
}