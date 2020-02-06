using System;
using System.Net;
using System.Net.Sockets;

namespace TimetableScreen.Configurator.Models
{
    public class NetworkTransport
    {
        private TcpListener listener;

        public event EventHandler<ResponseEventArgs> DataRecieved;

        public void Send(IPAddress iPAddress, ushort port, byte[] data)
        {
            using var client = new TcpClient();

            client.Connect(iPAddress, port);

            var stream = client.GetStream();

            byte[] size = BitConverter.GetBytes(data.Length);

            stream.Write(size, 0, size.Length);
            stream.Write(data, 0, data.Length);

            client.Close();
        }

        public void StartServer(IPAddress iPAddress, ushort port)
        {
            listener = new TcpListener(iPAddress, port);

            listener.Start();

            while (true)
            {
                using var client = listener.AcceptTcpClient();
                var stream = client.GetStream();

                var lengthBytes = new byte[sizeof(int)];

                stream.Read(lengthBytes, 0, lengthBytes.Length);

                var length = BitConverter.ToInt32(lengthBytes);

                var data = new byte[length];

                stream.Read(data, 0, length);

                client.Close();

                DataRecieved(null, new ResponseEventArgs(data));
            }

            listener.Stop();
        }

        public void StopServer()
        {
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




