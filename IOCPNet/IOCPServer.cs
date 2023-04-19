using System;
using System.Net;
using System.Net.Sockets;

namespace MGNet
{
    /// <summary>
    /// IOCP服务器: 接受客户端
    /// </summary>
	public class IOCPServer
	{
        private Socket socket;
        private SocketAsyncEventArgs args;
        private int backlog = 100;

        public IOCPServer()
		{
			args = new SocketAsyncEventArgs();
			args.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
		}

		public void StartAsServer(string ip, int port, int maxConnectCount)
		{
            IPEndPoint pt = new IPEndPoint(IPAddress.Parse(ip), port);
            socket = new Socket(pt.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(pt);
            socket.Listen(backlog);

            IOCPTool.ColorLog(IOCPLogColor.Green, "服务器开始接受...");
            StartAccept();
        }

        private void StartAccept()
		{
            bool suspend = socket.AcceptAsync(args);
            if (!suspend)
            {
                ProcessAccept();
            }
        }

        private void ProcessAccept()
		{

		}

        private void IO_Completed(object sender, SocketAsyncEventArgs e)
        {
			ProcessAccept();
        }
    }
}

