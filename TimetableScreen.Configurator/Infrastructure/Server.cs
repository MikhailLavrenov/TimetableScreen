using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace TimetableScreen.Configurator.Infrastructure
{

    public class Server
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

        public void Start(IPAddress iPAddress, ushort port)
        {
            Task.Run(() =>
            {
                listener = new TcpListener(iPAddress, port);
                listener.Start();
                isRunning = true;

                while (isRunning)
                {
                    try
                    {
                        using var client = listener.AcceptTcpClient();
                        Task.Run(() => ProcessConnection(client));
                    }
                    catch (SocketException ex) when (ex.ErrorCode == 10004)
                    { }
                }
            });
        }

        private void ProcessConnection(TcpClient client)
        {
            var stream = client.GetStream();

            var lengthBytes = new byte[sizeof(int)];
            var procedureBytes = new byte[sizeof(int)];

            stream.Read(procedureBytes, 0, procedureBytes.Length);

            var procedureId = BitConverter.ToInt32(lengthBytes);

            var data = new byte[length];

            stream.Read(data, 0, length);

            stream.Close();
            client.Close();

            DataRecieved(null, new ResponseEventArgs(data));
        }

        public void Stop()
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

    public enum Procedures : int
    {
        none=0,
        recieve=1,

    }
}




