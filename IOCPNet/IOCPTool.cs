using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace MGNet
{
    public enum IOCPLogColor
    {
        None,
        Red,
        Green,
        Blue,
        Cyan,
        Magenta,
        Yellow,
    }

    /// <summary>
    /// 工具类
    /// </summary>
	public class IOCPTool
    {
        #region LOG
        public static Action<string> LogFunc;
        public static Action<string> WarnFunc;
        public static Action<string> ErrorFunc;
        public static Action<IOCPLogColor, string> ColorLogFunc;

        public static void Error(string msg, params object[] args)
        {
            msg = string.Format(msg, args);
            if (ErrorFunc != null)
            {
                ErrorFunc(msg);
            }
            else
            {
                ConsoleLog(msg, IOCPLogColor.Red);
            }
        }

        public static void Warn(string msg, params object[] args)
        {
            msg = string.Format(msg, args);
            if (WarnFunc != null)
            {
                WarnFunc(msg);
            }
            else
            {
                ConsoleLog(msg, IOCPLogColor.Yellow);
            }
        }

        public static void Log(string msg, params object[] args)
        {
            msg = string.Format(msg, args);
            if (LogFunc != null)
            {
                LogFunc(msg);
            }
            else
            {
                ConsoleLog(msg, IOCPLogColor.None);
            }
        }

        public static void ColorLog(IOCPLogColor color, string msg, params object[] args)
        {
            msg = string.Format(msg, args);
            if (ColorLogFunc != null)
            {
                ColorLogFunc(color, msg);
            }
            else
            {
                ConsoleLog(msg, color);
            }
        }

        private static void ConsoleLog(string msg, IOCPLogColor color)
		{
			int threadID = Thread.CurrentThread.ManagedThreadId;
			msg = string.Format("Thread:{0} {1}", threadID, msg);
			switch (color)
			{
				case IOCPLogColor.Red:
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine(msg);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case IOCPLogColor.Green:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(msg);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case IOCPLogColor.Blue:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(msg);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case IOCPLogColor.Cyan:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(msg);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case IOCPLogColor.Magenta:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine(msg);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case IOCPLogColor.Yellow:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(msg);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                default:
                    Console.WriteLine(msg);
                    break;
            }
		}
        #endregion

        /// <summary>
        /// 分割字符数组
        /// </summary>
        /// <param name="bytesList"></param>
        /// <returns></returns>
        public static byte[] SplitBytes(ref List<byte> bytesList)
        {
            byte[] buff = null;
            if (bytesList.Count > 4)
            {
                // head(int) + data
                byte[] data = bytesList.ToArray();
                int len = BitConverter.ToInt32(data, 0);
                if (bytesList.Count > len + 4)
                {
                    buff = new byte[len];
                    Buffer.BlockCopy(data, 4, buff, 0, len);
                    bytesList.RemoveRange(0, len + 4);
                }
            }
            return buff;
        }

        /// <summary>
        /// 序列化消息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static byte[] Serialize(IOCPMsg msg)
        {
            byte[] data = null;
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                bf.Serialize(ms, msg);
                ms.Seek(0, SeekOrigin.Begin);
                data = ms.ToArray();
            }
            catch(SerializationException e)
            {
                Error("Fail to serialze Reson:{0}", e.Message);
            }
            finally
            {
                ms.Close();
            }
            return data;
        }

        /// <summary>
        /// 反序列化消息
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static IOCPMsg DeSerialize(byte[] bytes)
        {
            IOCPMsg msg = null;
            MemoryStream ms = new MemoryStream(bytes);
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                msg = (IOCPMsg)bf.Deserialize(ms);
            }
            catch (SerializationException e)
            {
                Error("Fail to serialze Reson:{0}, ByteLength", e.Message, bytes.Length);
            }
            finally
            {
                ms.Close();
            }
            return msg;
        }
    }
}

