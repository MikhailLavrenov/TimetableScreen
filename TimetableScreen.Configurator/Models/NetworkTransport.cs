using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace TimetableScreen.Configurator.Models
{
    [Serializable]
    public class NetworkTransport
    {
        private TcpListener listener;
        private bool isRunning;

        public event EventHandler<ResponseEventArgs> DataRecieved;

        public static void Send(IPAddress iPAddress, ushort port, byte[] data)
        {
            using var client = new TcpClient();

            client.Connect(iPAddress, port);

            var stream = client.GetStream();

            byte[] size = BitConverter.GetBytes(data.Length);

            stream.Write(size, 0, size.Length);
            stream.Write(data, 0, data.Length);

            stream.Close();
            client.Close();
        }

        public void StartServer(IPAddress iPAddress, ushort port)
        {           
            Task.Run(() =>
            {
                listener = new TcpListener(iPAddress, port);
                listener.Start();
                isRunning = true;

                while (isRunning)
                {
                        var client = listener.AcceptTcpClient();
                        var stream = client.GetStream();

                        var lengthBytes = new byte[sizeof(int)];

                        stream.Read(lengthBytes, 0, lengthBytes.Length);

                        var length = BitConverter.ToInt32(lengthBytes);

                        var data = new byte[length];

                        stream.Read(data, 0, length);

                        stream.Close();
                        client.Close();

                        DataRecieved(null, new ResponseEventArgs(data));
                }
            });
        }

        public void StopServer()
        {
            isRunning = false;
            listener?.Stop();
        }
    }

    public class ResponseEventArgs : EventArgs
    {
        public byte[] Buffer { get; set; }

        public ResponseEventArgs(byte[] buffer)
        {
            Buffer = buffer;
        }
    }
}




