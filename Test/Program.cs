using System;
using System.Net;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                var str = Console.ReadLine();

                var address = Dns.GetHostAddresses(str)[0];

                Console.WriteLine(address.ToString());
            }
        }
    }
}
