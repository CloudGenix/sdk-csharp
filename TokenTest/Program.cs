using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using CloudGenix;

namespace TokenTest
{
    class MainClass
    {
        private static CgnxController _Cgnx;

        public static void Main(string[] args)
        {
            try
            {
                Console.Write("Token: ");
                string token = ReadLine();
                if (String.IsNullOrEmpty(token)) return;

                _Cgnx = new CgnxController(token, true); 

                Console.Write("Endpoint [https://api.cloudgenix.com:443]: ");
                string endpoint = ReadLine();
                if (!String.IsNullOrEmpty(endpoint))
                {
                    _Cgnx.Endpoint = endpoint;
                }

                if (!_Cgnx.LoginWithToken())
                {
                    Console.WriteLine("Login failed");
                    return;
                }
                else
                {
                    Console.WriteLine("Login success");
                    return;
                }
            }
            finally
            {
                Console.WriteLine("Press ENTER to exit");
                Console.ReadLine();
            }
        }

        private static string ReadLine()
        {
            Stream inputStream = Console.OpenStandardInput(65536);
            byte[] bytes = new byte[65536];
            int outputLength = inputStream.Read(bytes, 0, 65536);
            //Console.WriteLine(outputLength);
            char[] chars = Encoding.UTF7.GetChars(bytes, 0, outputLength);
            string ret = new string(chars).Trim();
            ret = Regex.Replace(ret, @"\t|\n|\r", "");
            return ret;
        }
    }
}
