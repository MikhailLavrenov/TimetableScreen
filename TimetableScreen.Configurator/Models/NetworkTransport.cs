using System;
using System.Net;
using System.Net.Sockets;

namespace TimetableScreen.Configurator.Models
{
    public class NetworkTransport
    {
        public IPAddress RecieveAddress { get; set; } = IPAddress.Any;
        public string RemoteAddress { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 8642;
        public bool recieveNewConnections { get; set; } = true;

        public event EventHandler<ResponseEventArgs> DataRecieved;

        public void Send(byte[] data)
        {
            try
            {
                var client = new TcpClient();

                client.Connect(RemoteAddress, Port);

                var stream = client.GetStream();

                byte[] size = BitConverter.GetBytes(data.Length);

                stream.Write(size, 0, size.Length);
                stream.Write(data, 0, data.Length);

                client.Close();
            }
            catch (Exception)
            { }
        }

        public void BeginRecieve()
        {
            try
            {
                var listener = new TcpListener(RecieveAddress, Port);

                listener.Start();

                while (recieveNewConnections)
                {
                    var client = listener.AcceptTcpClient();
                    var stream = client.GetStream();

                    var lengthBytes = new byte[sizeof(int)];

                    stream.Read(lengthBytes, 0, lengthBytes.Length);

                    var length = BitConverter.ToInt32(lengthBytes);

                    var data = new byte[length];

                    stream.Read(data, 0, length);

                    DataRecieved(null, new ResponseEventArgs(data));

                    client.Close();
                }

                listener.Stop();
            }
            catch (Exception)
            { }
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

    public enum ConnectionStatus
    {
        None = 0,
        Success = 1,
        Fail = 2,
    }

}




