using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using OwinSelfhostSample;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:9000/";

            // Start OWIN host 
            using (WebApp.Start<Startup>(url: baseAddress))
            { 
                Console.WriteLine("the server works on http://localhost:9000/");
                
                Console.ReadLine();

            }

        }

    }
}
