using Back_end.Data;
using Microsoft.Extensions.Configuration;
using MicroWebServer.WebServer;
using MicroWebServer.WebServer.IO;
using MicroWebServer.WebServer.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

namespace Back_end
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        private static Business.Information information;
        public static void LoadConfiguration()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables().Build();
        }
        public static void Index(Requests requests, Response response)
        {
            response.sendJson(information.GetAllInformation(), 200);
        }
        public static void GetItem(Requests requests, Response response)
        {
            int id = int.Parse(requests.getArg("id", "0"));
            response.sendJson(information.Read(id), 200);
        }
        public static void Create(Requests requests, Response response)
        {
            Data.Information POST = JsonConvert.DeserializeObject<Data.Information>(requests.body);
            response.sendJson(information.Create(POST), 200);
        }
        public static void Delete(Requests requests, Response response)
        {
            int id = int.Parse(requests.getArg("id", null));
            var value = information.Delete(id);
            response.sendJson(new Dictionary<string, string>() { { "status", value.ToString() } }, 200);
        }
        public static void Find(Requests requests, Response response)
        {
            string name = requests.getArg("name", null);
            response.sendJson(information.Find(name), 200);
        }
        public static void Update(Requests requests, Response response)
        {
            int id = int.Parse(requests.getArg("id", null));
            Data.Information POST = JsonConvert.DeserializeObject<Data.Information>(requests.body);
            response.sendJson(information.Update(id, POST), 200);
        }
        public static (Requests,Response) AccessControllMiddleware(Requests requests, Response response)
        {
            response.header["Access-Control-Allow-Origin"] = "*";
            response.header["Access-Control-Allow-Headers"] = "Content-Type, Content-Length, Accept-Encoding";
            return (requests, response);
        }
        static void Main()
        {
            LoadConfiguration();
            Data.PeoplesContext peoplesContext = new PeoplesContext();
            information = new Business.Information(peoplesContext);

            ConsoleLog consoleLog = new ConsoleLog();
            Dictionary<string, Action<Requests, Response>> urlPatterns = new Dictionary<string, Action<Requests, Response>>()
            {
                {@"^\/$", Index },
                {@"^\/\?id\=[0-9]$", GetItem },
                {@"^\/create", Create },
                {@"^\/delete\?id\=[0-9]+$", Delete },
                {@"^\/find\?name\=[a-zA-Z]+$", Find },
                {@"^\/update\?id\=[0-9]+$", Update },
            };
            var serverConfiguration = Configuration.GetSection("Server").GetChildren().ToList();
            Server server = new Server(
                IPAddress.Parse(serverConfiguration[0].Value),
                int.Parse(serverConfiguration[1].Value),
                10,
                urlPatterns,
                consoleLog
                );
            server.Middlewares.Add(AccessControllMiddleware);
            if (server.Start())
            {
                consoleLog.Informational("Started");
            }
            Thread.CurrentThread.Join();
        }
    }
}
