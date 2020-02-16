using System;
using System.IO;
using System.Net;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var stream = new MemoryStream();

            var arr = new byte[] {1,2,3,4,5,6,7,8,9,10 };

            foreach (var item in arr)
                Console.WriteLine(item);
            Console.WriteLine("------");

            stream.WriteByte(arr[0]);
            stream.Write(arr, 1, arr.Length - 1);

            stream.Position = 0;
            var res = new byte[5];
            stream.Read(res, 0, 5);
            var sing = stream.ReadByte();

            foreach (var item in res)
                Console.WriteLine(item);

            Console.WriteLine("------");
            Console.WriteLine(sing);

            Console.ReadKey();
        }
    }
}
