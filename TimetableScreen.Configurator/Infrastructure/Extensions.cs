using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace TimetableScreen.Configurator.Infrastructure
{
    public static class Extensions
    {
        public static string GetString(this byte[] byteArray)
        {
            return Encoding.UTF8.GetString(byteArray);
        }

        public static byte[] GetBytes(this string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        public static string StreamToString(Stream stream)
        {
            stream.Position = 0;
            using var reader = new StreamReader(stream, Encoding.UTF8);

            return reader.ReadToEnd();
        }

        public static Stream StringToStream(string src)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(src);
            return new MemoryStream(byteArray);
        }

        public static T FindVisualParent<T>(this FrameworkElement element) where T : FrameworkElement
        {
            while (element != null)
            {
                element = VisualTreeHelper.GetParent(element) as FrameworkElement;

                if (element is T correctlyTyped)
                    return correctlyTyped;
            }

            return null;
        }

        public static FrameworkElement FindLogicalParent(this FrameworkElement element, string foundName)
        {
            while (element != null)
            {
                if (element.Name == foundName)
                    return element;

                element = element.Parent as FrameworkElement;
            }

            return null;
        }

        public static T FindVisualChild<T>(this FrameworkElement element, string name) where T : FrameworkElement
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(element);

            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(element, i) as FrameworkElement;

                if (child is T correctlyTyped && child.Name == name)
                    return correctlyTyped;

                var result = FindVisualChild<T>(child, name);

                if (result != null)
                    return result;
            }

            return null;
        }

        public static void FindVisualChilds<T>(this FrameworkElement element, string name, ref List<T> result) where T : FrameworkElement
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(element);

            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(element, i) as FrameworkElement;

                if (child is T correctlyTyped && child.Name == name)
                    result.Add(correctlyTyped);
                else
                    child.FindVisualChilds(name, ref result);
            }
        }

        public static object Deserialize(this byte[] array, Type type)
        {
            var mStream = new MemoryStream();
            mStream.Write(array, 0, array.Length);
            mStream.Position = 0;

            var formatter = new XmlSerializer(type);
            return formatter.Deserialize(mStream);
        }

        public static byte[] Serialize<T>(this T obj) where T : class
        {
            var mStream = new MemoryStream();
            var formatter = new XmlSerializer(typeof(T));
            formatter.Serialize(mStream, obj);
            return mStream.ToArray();
        }

        public static void WriteWithSize(this NetworkStream stream, byte[] data)
        {
            var size = BitConverter.GetBytes(data.Length);

            stream.Write(size, 0, size.Length);
            stream.Write(data, 0, data.Length);
        }

        public static byte[] ReadWithSize(this NetworkStream stream)
        {
            var sBytes = new byte[sizeof(int)];
            stream.Read(sBytes, 0, sBytes.Length);
            var size = BitConverter.ToInt32(sBytes);

            var data = new byte[size];
            stream.Read(data, 0, data.Length);

            return data;
        }
    }
}
