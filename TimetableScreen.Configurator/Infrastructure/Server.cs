using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TimetableScreen.Configurator.Models;

namespace TimetableScreen.Configurator.Infrastructure
{

    public class Server
    {
        private TcpListener listener;
        private bool isRunning;
        private int nextId = 0;
        private Dictionary<int, TcpClient> awaitingClients;

        public event EventHandler<NetworkTransmissionEventArgs> NetworkTransmissionEvent;

        public Server()
        {
            awaitingClients = new Dictionary<int, TcpClient>();
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
                        //Task.Run(() => AcceptClient(client));
                        AcceptClient(client);
                    }
                    catch (SocketException ex) when (ex.ErrorCode == 10004)
                    { }
                }
            });
        }

        private void AcceptClient(TcpClient client)
        {
            var stream = client.GetStream();

            var operation = (Operation)stream.ReadByte();

            var bytes = new byte[sizeof(int)];
            stream.Read(bytes, 0, bytes.Length);
            var size = BitConverter.ToInt32(bytes);

            bytes = new byte[size];
            stream.Read(bytes, 0, bytes.Length);
            var typeName = Encoding.UTF8.GetString(bytes);
            var objectType = Type.GetType(typeName);

            var args = new NetworkTransmissionEventArgs
            {
                Procedure = operation,
                ObjectType = objectType,
            };

            if (operation == Operation.serverMustRecieve)
            {
                bytes = new byte[sizeof(int)];
                stream.Read(bytes, 0, bytes.Length);
                size = BitConverter.ToInt32(bytes);

                bytes = new byte[size];
                stream.Read(bytes, 0, bytes.Length);

                args.Object = bytes.Deserialize(objectType);

                stream.Close();
                client.Close();
            }
            else
            {
                args.RequestId = nextId;
                awaitingClients.Add(nextId, client);
                nextId++;
            }

            NetworkTransmissionEvent(null, args);
        }

        public void SendRequestedObject<T>(object recipient, T obj) where T : class
        {
            var data = obj.Serialize();

            var index = (int)recipient;
            var client = awaitingClients[index];
            var stream = client.GetStream();
            awaitingClients.Remove(index);

            var size = BitConverter.GetBytes(data.Length);

            stream.Write(size, 0, size.Length);
            stream.Write(data, 0, data.Length);

            stream.Close();
            client.Close();
        }

        public void Stop()
        {
            isRunning = false;
            listener?.Stop();
        }
    }

}




