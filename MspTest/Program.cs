using System;
using System.Collections.Generic;
using CloudGenix;
using CloudGenix.Classes;

namespace MspTest
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
                    Console.WriteLine("Auth token : " + _Cgnx.AuthToken);
                    Console.WriteLine("Tenant ID  : " + _Cgnx.TenantId);
                }

                Console.WriteLine("Retrieving clients...");
                List<Client> clients = new List<Client>();
                if (!_Cgnx.GetClients(out clients))
                {
                    Console.WriteLine("Unable to retrieve clients");
                    return;
                }
                else
                {
                    Console.WriteLine("Retrieved " + clients.Count + " client(s):");
                    foreach (Client curr in clients)
                    {
                        Console.WriteLine("  " + curr.Id + ": " + curr.Name + " [" + curr.CanonicalName + "]");
                    }
                }

                Console.Write("ID to emulate: ");
                string id = Console.ReadLine();
                if (!_Cgnx.LoginAsClient(id))
                {
                    Console.WriteLine("Unable to login as client ID " + id);
                    return;
                }
                else
                {
                    Console.WriteLine("Successfully logged in as client ID " + id);
                    Console.WriteLine("Auth token : " + _Cgnx.AuthToken);
                    Console.WriteLine("Tenant ID  : " + _Cgnx.TenantId);
                }

                List<Site> sites = new List<Site>();
                if (!_Cgnx.GetSites(out sites))
                {
                    Console.WriteLine("Unable to retrieve sites");
                    return;
                }
                else
                {
                    Console.WriteLine("Retrieved " + sites.Count + " site(s)");
                    foreach (Site curr in sites)
                    {
                        Console.WriteLine("  " + curr.Id + " " + curr.Name);
                    }
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
