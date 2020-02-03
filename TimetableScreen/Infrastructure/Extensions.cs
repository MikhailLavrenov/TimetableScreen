using System;
using System.Collections.Generic;
using System.Text;

namespace TimetableScreen.Infrastructure
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

    }
}
