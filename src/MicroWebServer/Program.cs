using MicroWebServer.WebServer;
using MicroWebServer.WebServer.IO;
using MicroWebServer.WebServer.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Newtonsoft.Json;
using MicroWebServer.WebServer.Utility;

namespace MicroWebServer
{
    class Program
    {
        public static void Info(Requests requests, Response response)
        {
            if (requests.requestInfo["method"] == "POST")
            {
                var x = JsonConvert.DeserializeObject<Dictionary<string,string>>(requests.body);
                Console.WriteLine(x["name"]);
                response.send200Ok("Hello World!", response.extensions["txt"]);
            }
            else
            {
                Dictionary<string, string> myInfo = new Dictionary<string, string>()
                {
                    {"name",requests.getArg("name","null")},
                    {"age",requests.getArg("age","null") },
                    {"github","https://github.com/xiosec" },
                };
                response.sendJson(myInfo, 200);
            }
        }
        public static void Index(Requests requests, Response response)
        {
            response.header["time"] = DateTime.Now.ToString();
            response.cookie["name"] = "xiosec";
            response.setSecurityHeader();
            Console.WriteLine(requests.header["time"]);
            Console.WriteLine(requests.cookie["name"]);
            response.send200Ok("Hello World!", response.extensions["html"]);
        }
        public static void Programer(Requests requests, Response response)
        {
            if (requests.requestInfo["method"] == "POST")
            {
                response.send200Ok("i'm xiosec", response.extensions["html"]);
            }
            else
            {
                response.send200Ok("method not supported", response.extensions["html"]);
            }
        }
        public static void Test(Requests requests,Response response)
        {
            string urlEncode = new HttpUtilitys().UrlDecode(requests.getArg("input", null));
            response.send200Ok(response.safeResponse(urlEncode), response.extensions["html"]);
            //response.send200Ok(response.safeResponse("<script>alert('hello')</script>"), response.extensions["html"]);

        }
        static void Main(string[] args)
        {
            ConsoleLog consoleLog = new ConsoleLog();
            Dictionary<string, Action<Requests, Response>> urlPatterns = new Dictionary<string, Action<Requests, Response>>()
            {
                {@"^\/$",Index },
                {@"^\/programer$", Programer},
                {@"^\/info\?name\=[a-z]+\&age=\d+$", Info},
                {@"^\/test\?input\=*.+$", Test}
            };

            Server server = new Server(IPAddress.Parse("127.0.0.1"), 8080, 10, urlPatterns, consoleLog);
            if (server.Start())
            {
                consoleLog.Informational("Started");
            }
            Thread.CurrentThread.Join();
        }
    }
}
