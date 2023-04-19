using System;
using System.Net;
using System.Net.Sockets;

namespace MGNet
{
	/// <summary>
	/// IOCP客户端: 连接服务器
	/// </summary>
	public class IOCPClient
	{
        private Socket socket;
        private SocketAsyncEventArgs args;

        public IOCPClient()
		{
            args = new SocketAsyncEventArgs();
            args.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
		}

		public void StartAsClient(string ip, int port)
		{
			IPEndPoint pt = new IPEndPoint(IPAddress.Parse(ip), port);
			socket = new Socket(pt.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			args.RemoteEndPoint = pt;

			IOCPTool.ColorLog(IOCPLogColor.Green, "客户端开始连接...");

			StartConnect();
		}

        private void StartConnect()
		{
            /*
             * 异步事件没有挂起：返回false，连接成功
			 * 异步事件挂起：返回true，连接失败，等待异步完成后调用 IO_Completed
			 */
            bool suspend = socket.ConnectAsync(args);
			if (!suspend)
			{
                ProcessConnected();
			}
        }

        private void ProcessConnected()
		{

		}

        private void IO_Completed(object sender, SocketAsyncEventArgs e)
		{
            ProcessConnected();
		}
	}
}

