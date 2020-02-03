using System;
using System.Net;
using System.Net.Sockets;


namespace TimetableScreen.Models
{
    public class NetworkTransport
    {
        public string LocalAddress { get; set; } = "127.0.0.1";
        public int LocalPort { get; set; } = 9753;
        public string RemoteHost { get; set; } = "127.0.0.1";
        public int RemotePort { get; set; } = 8642;
        public bool recieveConnections { get; set; } = true;

        public event EventHandler<ResponseEventArgs> DataRecieved;


        public void Send(byte[] data)
        {


            try
            {
                var client = new TcpClient();

                client.Connect(RemoteHost, RemotePort);

                var stream = client.GetStream();

                stream.Write(data, 0, data.Length);

                //byte[] bb = new byte[100];
                //int k = stream.Read(bb, 0, 100);

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
                var listener = new TcpListener(ipAddress, LocalPort);

                listener.Start();

                while (recieveConnections)
                {
                    var socket = listener.AcceptSocket();

                    byte[] buffer = new byte[int.MaxValue];
                    int k = socket.Receive(buffer);

                    DataRecieved(null, new ResponseEventArgs(buffer));

                    //var encoder = new ASCIIEncoding();
                    //socket.Send(encoder.GetBytes("The string was recieved by the server."));

                    socket.Close();
                }

                listener.Stop();
            }
            catch (Exception)
            {
            }
        }

    }


    public class ResponseEventArgs : EventArgs
    {
        public byte[] Buffer { get; set; }

        public ResponseEventArgs(byte[] buffer)
        {
            this.Buffer = buffer;
        }
    }

}




