using System;
using CloudGenix;

namespace SamlTest
{
    class MainClass
    {
        private static CgnxController _Cgnx;

        public static void Main(string[] args)
        {
            try
            {
                Console.Write("Email address: ");
                string email = Console.ReadLine();
                if (String.IsNullOrEmpty(email)) return;

                _Cgnx = new CgnxController(email);

                string samlUrl = null;
                if (!_Cgnx.LoginSamlStart(out samlUrl))
                {
                    Console.WriteLine("Login failed");
                    return;
                }

                Console.WriteLine("Copy and paste the following URL into your browser.  Press ENTER to continue.");
                Console.WriteLine(samlUrl);
                Console.ReadLine();

                if (!_Cgnx.LoginSamlFinish())
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
    }
}
