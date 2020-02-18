using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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
            try
            {
                var stream = client.GetStream();

                var operation = (Operation)stream.ReadByte();

                if (operation != Operation.RecieveFromServer && operation != Operation.SendToServer)
                {
                    client.Close();
                    return;
                }

                var type = stream.ReadWithSize();
                var typeName = Encoding.UTF8.GetString(type);
                var objectType = Type.GetType(typeName);

                var args = new NetworkTransmissionEventArgs(operation, objectType);

                if (operation == Operation.SendToServer)
                {
                    var data = stream.ReadWithSize();
                    args.Object = data.Deserialize(objectType);

                    client.Close();
                }
                else if (operation == Operation.RecieveFromServer)
                {
                    args.RequestId = nextId;
                    awaitingClients.Add(nextId, client);
                    nextId++;
                }
                else return;

                NetworkTransmissionEvent(null, args);
            }
            catch (Exception)
            {
                client.Close();
            }
        }

        public void SendRequestedObject<T>(object recipient, T obj) where T : class
        {
            TcpClient client = null;

            try
            {
                var index = (int)recipient;
                client = awaitingClients[index];
                var stream = client.GetStream();
                awaitingClients.Remove(index);

                var data = obj.Serialize();
                stream.WriteWithSize(data);
            }
            catch (Exception)
            { }
            finally
            {
                client?.Close();
            }
        }

        public void Stop()
        {
            if (isRunning)
            {
                isRunning = false;
                listener?.Stop();
            }
        }
    }

}




