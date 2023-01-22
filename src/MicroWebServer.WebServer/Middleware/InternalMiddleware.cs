using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MicroWebServer.WebServer.IO;
namespace MicroWebServer.WebServer.Middleware
{
    class InternalMiddleware
    {
        public (Requests,Response) TimeHeader(Requests requests,Response response)
        {
            response.header["time"] = DateTime.Now.ToString();
            return (requests,response);
        }
        public (Requests,Response) RequestInfo(Requests requests,Response response)
        {
            Console.WriteLine($"*Path : {requests.requestInfo["path"]}\r\n" +
                              $"*Method : {requests.requestInfo["method"]}\r\n" +
                              $"*Http Version : {requests.requestInfo["httpVersion"]}");
            return (requests, response);
        }
    }
}
