using MicroWebServer.WebServer.IO;
using MicroWebServer.WebServer.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using MicroWebServer.WebServer.Middleware;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MicroWebServer.WebServer
{
    public class Server
    {
        private Encoding charEncoder = Encoding.UTF8;
        private Socket serverSocket;
        private IPAddress ipAddress;
        private Regex regex;
        private int maxOfConnections { get; set; }
        private int timeout { get; set; }
        private ILog _log { get; set; }
        private int port { get; set; }
        public bool running = false;
        private Dictionary<string, Action<Requests, Response>> routeTable;
        private InternalMiddleware internalMiddleware;
        public List<Func<Requests, Response, (Requests, Response)>> Middlewares;
        public Server(IPAddress ipAddress, int port, int maxOfConnections, Dictionary<string, Action<Requests, Response>> routing, ConsoleLog consoleLog)
        {
            this.ipAddress = ipAddress;
            this.port = port;

            this.maxOfConnections = maxOfConnections;
            this.timeout = 8;

            internalMiddleware = new InternalMiddleware();
            Middlewares = new List<Func<Requests, Response, (Requests, Response)>>()
            {
                internalMiddleware.RequestInfo,
                internalMiddleware.TimeHeader,
            };
            routeTable = routing;
            _log = consoleLog;
        }
        public Server(IPAddress ipAddress, int port, int maxOfConnections, Dictionary<string, Action<Requests, Response>> routing, SysLog sysLog)
        {
            this.ipAddress = ipAddress;
            this.port = port;

            this.maxOfConnections = maxOfConnections;
            this.timeout = 8;

            internalMiddleware = new InternalMiddleware();

            routeTable = routing;
            _log = sysLog;
        }
        private void Banner()
        {
            if (running)
            {
                Console.WriteLine("The web server was successfully launched.\nThe information is as follows : \n"
                                  + $"*Web server runtime : {DateTime.Now} \n"
                                  + $"*Web Server ip Address : {ipAddress} \n"
                                  + $"*web Server Port : {port} \n"
                                  + $"*Web server start status : {running} \n"
                                  + $"*Max Of Connection : {maxOfConnections} \n"
                                  + $"*Connection Time out : {timeout} Seconds\n"
                                  + $"*Url : http://{ipAddress}:{port}\n"
                                  + $"*Route Table row Count : {routeTable.Count}\n\r\n\r");
            }
        }
        public bool Start()
        {
            if (running) return false;
            try
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(new IPEndPoint(ipAddress, port));
                serverSocket.Listen(maxOfConnections);
                serverSocket.ReceiveTimeout = timeout;
                serverSocket.SendTimeout = timeout;
                running = true;
            }
            catch
            {
                _log.Critical("Problem setting up the server");
                return false;
            }

            Task.Run(() =>
            {
                while (running)
                {
                    Socket clientSocket;
                    try
                    {
                        clientSocket = serverSocket.Accept();
                        Task.Run(() =>
                        {
                            clientSocket.ReceiveTimeout = timeout;
                            clientSocket.SendTimeout = timeout;
                            try { handleTheRequest(clientSocket); }
                            catch
                            {
                                try { clientSocket.Close(); } catch { }
                            }
                        });
                    }
                    catch { _log.Error("Problem Run Request Handler"); }
                }
            });
            Banner();
            return true;
        }
        public void stop()
        {
            if (running)
            {
                running = false;
                try
                {
                    serverSocket.Close();
                }
                catch { _log.Error("Problem closing the socket"); }
                serverSocket = null;
            }
        }
        private void handleTheRequest(Socket clientSocket)
        {
            byte[] buffer = new byte[1024];
            int receivedBCount = clientSocket.Receive(buffer);
            string strReceived = charEncoder.GetString(buffer, 0, receivedBCount);
            string httpMethod = strReceived.Substring(0, strReceived.IndexOf(" "));
            int start = strReceived.IndexOf(httpMethod) + httpMethod.Length + 1;
            int length = strReceived.LastIndexOf("HTTP") - start - 1;
            string requestedUrl = strReceived.Substring(start, length);
            _log.Informational($"{requestedUrl} {httpMethod} {length}");
            bool isValid = false;
            foreach (var (key,_) in routeTable)
            {
                regex = new Regex(key);
                if (regex.IsMatch(requestedUrl))
                {
                    isValid = true;
                    Response response = new Response(clientSocket);
                    Requests requests = new Requests(strReceived);
                    foreach (var Middleware in Middlewares)
                    {
                        (requests, response) = Middleware(requests, response);
                    }
                    routeTable[key](requests, response);
                    break;
                }
            }
            if(!isValid)
            {
                _log.Warning($"path {requestedUrl} not found");
                new Response(clientSocket).sendNotFound("Not Found !!!", "text/html");
            }
        }
    }
}
