using System;
using System.Net;
using System.Net.Sockets;
using TimetableScreen.Infrastructure;

namespace TimetableScreen.Models
{
    public class NetworkTransport
    {
        string statusMessage = "success";

        public string LocalAddress { get; set; } = "127.0.0.1";
        public string RemoteHost { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 8642;
        public bool recieveNewConnections { get; set; } = true;

        public event EventHandler<ResponseEventArgs> DataRecieved;


        public void Send(byte[] data)
        {
            try
            {
                var client = new TcpClient();

                client.Connect(RemoteHost, Port);

                var stream = client.GetStream();

                stream.Write(data, 0, data.Length);

                byte[] resBytes = new byte[statusMessage.Length];
                stream.Read(resBytes, 0, resBytes.Length);

                if (resBytes.GetString() != statusMessage)
                    throw new SocketException();

                stream.Close();
                client.Close();
            }
            catch (Exception)
            { }
        }

        public void Recieve()
        {
            try
            {
                var ipAddress = IPAddress.Parse(LocalAddress);
                var listener = new TcpListener(ipAddress, Port);

                listener.Start();

                while (recieveNewConnections)
                {
                    var socket = listener.AcceptSocket();

                    var buffer = new byte[socket.Available];
                    socket.Receive(buffer);

                    DataRecieved(null, new ResponseEventArgs(buffer));
                    socket.Send(statusMessage.GetBytes());

                    socket.Close();
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




