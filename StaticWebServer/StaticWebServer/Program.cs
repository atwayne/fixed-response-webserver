using System;
using System.Configuration;

namespace StaticWebServer
{
    class Program
    {
        static void Main()
        {
            var endpoint = ConfigurationManager.AppSettings["Endpoint"];
            var resourceFilePath = ConfigurationManager.AppSettings["ResourceFile"];
            var contentType = ConfigurationManager.AppSettings["ContentType"];
            var server = new FixedResponseWebServer(endpoint, resourceFilePath, contentType);
            server.Start();
            Console.ReadLine();
        }
    }
}
