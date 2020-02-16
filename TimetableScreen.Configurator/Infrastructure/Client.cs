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

            var typeName = typeof(T).FullName;
            var type = Encoding.UTF8.GetBytes(typeName);
            var size = BitConverter.GetBytes(type.Length);

            stream.WriteByte((byte)Operation.serverMustRecieve);
            stream.Write(size, 0, size.Length);
            stream.Write(type, 0, type.Length);

            var data = obj.Serialize();
            size = BitConverter.GetBytes(data.Length);            

            stream.Write(size, 0, size.Length);
            stream.Write(data, 0, data.Length);

            stream.Close();
            client.Close();
        }

        public static T Recieve<T>(string address, ushort port) where T:class
        {
            var client = new TcpClient();
            client.Connect(GetIpAddress(address), port);
            var stream = client.GetStream();
           
            var typeName = typeof(T).FullName;
            var type = Encoding.UTF8.GetBytes(typeName);
            var size = BitConverter.GetBytes(type.Length);

            stream.WriteByte((byte)Operation.serverMustSend);
            stream.Write(size, 0, size.Length);
            stream.Write(type, 0, type.Length);

            size = new byte[sizeof(int)];
            stream.Read(size, 0, size.Length);
            var length = BitConverter.ToInt32(size);

            var bytes = new byte[length];
            stream.Read(bytes,0,bytes.Length);

            stream.Close();
            client.Close();

            return (T)bytes.Deserialize(typeof(T));
        }
    }
}
