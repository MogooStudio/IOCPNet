using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace MGNet
{
	public enum TokenState
	{
		None,
		Connected,
		Disconnected,
	}

    /// <summary>
    /// IOCPToken: 处理数据接收
    /// </summary>
	public class IOCPToken
	{
		public int tokenID;
        public TokenState tokenState = TokenState.None;

        private Socket socket;
        private SocketAsyncEventArgs recvArgs;
        private List<byte> readList = new List<byte>();
		
        public IOCPToken()
		{
			recvArgs = new SocketAsyncEventArgs();
			recvArgs.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
            recvArgs.SetBuffer(new byte[2048], 0, 2048);
        }

		public void InitToken(Socket socket)
		{
			this.socket = socket;
			this.tokenState = TokenState.Connected;
			OnConnected();
			StartAsyncRecv();
        }

        public void CloseToken()
        {
            //TODO
        }

        private void OnConnected()
        {
            IOCPTool.Log("连接成功...");
        }

        private void StartAsyncRecv()
		{
            bool suspend = socket.ReceiveAsync(recvArgs);
            if (!suspend)
            {
                ProcessRecv();
            }
        }

        private void ProcessRecv()
		{
            if (recvArgs.BytesTransferred > 0 && recvArgs.SocketError == SocketError.Success)
            {
                // 接收字节
                byte[] bytes = new byte[recvArgs.BytesTransferred];
                Buffer.BlockCopy(recvArgs.Buffer, 0, bytes, 0, recvArgs.BytesTransferred);
                readList.AddRange(bytes);
                // 处理字节
                ProcessByteList();
                // 继续接收
                StartAsyncRecv();
            }
            else
            {
                IOCPTool.Warn("Token: {0}, Close: {1}", tokenID, recvArgs.SocketError.ToString());
                CloseToken();
            }
		}

        private void ProcessByteList()
        {
            byte[] buff = IOCPTool.SplitBytes(ref readList);
            if (buff != null)
            {
                IOCPMsg msg = IOCPTool.DeSerialize(buff);
                OnReceiveMsg(msg);
                ProcessByteList();
            }
        }

        private void IO_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessRecv();
        }

        private void OnReceiveMsg(IOCPMsg msg)
        {
            IOCPTool.Log("收到数据：{0}", msg.hellomsg);
        }
    }
}

