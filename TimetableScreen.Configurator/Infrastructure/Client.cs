using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TimetableScreen.Configurator.Infrastructure
{
    public static class Client
    {
        private static IPAddress GetIpAddress(string address)
        {
            //мб socketException
            var res = Dns.GetHostAddresses(address).First(x => x.AddressFamily == AddressFamily.InterNetwork);
            return res;
        }

        public static void Send<T>(string address, ushort port, T obj) where T : class
        {
            var client = new TcpClient();

            try
            {
                client.Connect(GetIpAddress(address), port);
                var stream = client.GetStream();

                stream.WriteByte((byte)Operation.SendToServer);

                var typeName = typeof(T).FullName;
                var type = Encoding.UTF8.GetBytes(typeName);
                stream.WriteWithSize(type);

                var data = obj.Serialize();
                stream.WriteWithSize(data);
            }
            catch (Exception)
            { }
            finally
            {
                client.Close();
            }
        }

        public static T Recieve<T>(string address, ushort port) where T : class
        {
            var client = new TcpClient();

            try
            {
                client.Connect(GetIpAddress(address), port);
                var stream = client.GetStream();

                stream.WriteByte((byte)Operation.RecieveFromServer);

                var typeName = typeof(T).FullName;
                var type = Encoding.UTF8.GetBytes(typeName);
                stream.WriteWithSize(type);

                var data = stream.ReadWithSize();

                client.Close();

                return (T)data.Deserialize(typeof(T));
            }
            catch(Exception)
            {
                client.Close();
                return null;
            }

        }

        public static bool TestConnection(string address, ushort port)
        {
            using var client = new TcpClient();

            try
            {
                client.ConnectAsync(GetIpAddress(address), port).Wait(5000);
                var stream = client.GetStream();
                stream.WriteByte((byte)Operation.TestConnection);
                var connected = client.Connected;
                client.Close();

                return connected;
            }
            catch (Exception)
            {
                client.Close();
                return false;
            }
        }
    }
}
