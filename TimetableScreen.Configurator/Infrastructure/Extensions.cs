using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;

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


    }
}
