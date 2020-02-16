using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml.Serialization;

namespace TimetableScreen.Configurator.Infrastructure
{
    public static class Client
    {
        private static IPAddress GetIpAddress(string address)
        {
            //мб socketException
            var res= Dns.GetHostAddresses(address).First(x => x.AddressFamily == AddressFamily.InterNetwork);
            return res;
        }

        public static void Send<T>(string address, ushort port, T obj) where T:class
        {
            var client = new TcpClient();
            client.Connect(GetIpAddress(address), port);
            var stream = client.GetStream();

            stream.WriteByte((byte)Operation.SendToServer);

            var typeName = typeof(T).FullName;
            var type = Encoding.UTF8.GetBytes(typeName);
            stream.WriteWithSize(type);

            var data = obj.Serialize();
            stream.WriteWithSize(data);

            stream.Close();
            client.Close();
        }

        public static T Recieve<T>(string address, ushort port) where T:class
        {
            var client = new TcpClient();
            client.Connect(GetIpAddress(address), port);
            var stream = client.GetStream();

            stream.WriteByte((byte)Operation.RecieveFromServer);

            var typeName = typeof(T).FullName;
            var type = Encoding.UTF8.GetBytes(typeName);
            stream.WriteWithSize(type);

            var data = stream.ReadWithSize();

            stream.Close();
            client.Close();

            return (T)data.Deserialize(typeof(T));
        }
    }
}
