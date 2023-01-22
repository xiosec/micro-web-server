using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Web;

namespace MicroWebServer.WebServer.IO
{
    public class Response
    {
        private Encoding charEncoder = Encoding.UTF8;
        private Socket clientSocket;
        /// <summary>
        /// Media type
        /// </summary>
        public Dictionary<string, string> extensions = new Dictionary<string, string>()
        {
            { "htm", "text/html" },
            { "html", "text/html" },
            { "xml", "text/xml" },
            { "txt", "text/plain" },
            { "css", "text/css" },
            { "png", "image/png" },
            { "gif", "image/gif" },
            { "jpg", "image/jpg" },
            { "jpeg", "image/jpeg" },
            { "zip", "application/zip"},
            { "json","application/json" },
        };
        /// <summary>
        /// Status codes
        /// </summary>
        public Dictionary<int, string> statusCode = new Dictionary<int, string>()
        {
            {200, "OK" },
            {201, "Created" },
            {202, "Accepted" },
            {203, "Non-Authoritative Information" },
            {204, "No Content" },
            {205, "Reset Content" },
            {305, "Use Proxy" },
            {301, "Moved Permanently"},
            {308, "Permanent Redirect"},
            {400, "Bad Request" },
            {401, "Unauthorized" },
            {403, "Forbidden" },
            {404, "Not Found" },
            {405, "Method Not Allowed" },
            {406, "Not Acceptable" },
            {408, "Request Timeout" },
            {500, "Internal Server Error" },
            {501, "Not Implemented" },
            {502, "Bad Gateway" },
            {503, "Service Unavailable" },
            {504, "Gateway Timeout" },
            {505, "HTTP Version Not Supported" },
        };
        /// <summary>
        /// Dictionary for header sets
        /// </summary>
        public Dictionary<string, string> header = new Dictionary<string, string>();
        /// <summary>
        ///  Dictionary for cookie sets
        /// </summary>
        public Dictionary<string, string> cookie = new Dictionary<string, string>();
        public Response(Socket clientSocket)
        {
            this.clientSocket = clientSocket;
        }
        private string GenerateHeader()
        {
            if (header.Count < 1 && cookie.Count< 1)
            {
                return null;
            }
            var headerItem = from item in header
                        select item.Key + ": " + item.Value;

            var cookieItem = from item in cookie
                             select item.Key + "=" + item.Value;

            string cookieStr = cookie.Count>0 ? $"set-cookie:{string.Join("; ", cookieItem)}\r\n":null;

            return $"{cookieStr}{string.Join("\r\n", headerItem)}\r\n";
        }
        private void sendResponse(byte[] bContent, int responseCode, string contentType)
        {
            try
            {
                byte[] bHeader = charEncoder.GetBytes(
                                    $"HTTP/1.1 {responseCode} {statusCode[responseCode]}\r\n"
                                  + "Server: Micro Web Server\r\n"
                                  + "Content-Length: " + bContent.Length.ToString() + "\r\n"
                                  + "Connection: close\r\n"
                                  + GenerateHeader()
                                  + "Content-Type: " + contentType + "\r\n\r\n");
                clientSocket.Send(bHeader);
                clientSocket.Send(bContent);
                clientSocket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        /// <summary>
        /// Send information with status code 200
        /// </summary>
        /// <param name="bContent">Information to send to the client</param>
        /// <param name="contentType">Media type</param>
        public void send200Ok(string bContent, string contentType)
        {
            sendResponse(charEncoder.GetBytes(bContent), 200, contentType);
        }
        /// <summary>
        /// Page creation not found
        /// </summary>
        /// <param name="bContent">Information to send to the client</param>
        /// <param name="contentType">Media type</param>
        public void sendNotFound(string bContent, string contentType)
        {
            sendResponse(charEncoder.GetBytes(bContent), 404, contentType);
        }
        /// <summary>
        /// This function is used to send information with a custom status code
        /// </summary>
        /// <param name="bContent">Information to send to the client</param>
        /// <param name="statusCode">Answer status code</param>
        /// <param name="contentType">Media type</param>
        public void send(string bContent, int statusCode, string contentType)
        {
            sendResponse(charEncoder.GetBytes(bContent), statusCode, contentType);
        }
        /// <summary>
        /// Send a json string
        /// </summary>
        /// <param name="bContent">json string</param>
        /// <param name="statusCode">Answer status code</param>
        public void sendJson(string bContent, int statusCode)
        {
            sendResponse(charEncoder.GetBytes(bContent), statusCode, extensions["json"]);
        }
        /// <summary>
        /// Automatically converts the dictionary to json format
        /// </summary>
        /// <param name="bContent">dictionary</param>
        /// <param name="statusCode">Answer status code</param>
        public void sendJson(Dictionary<string, string> bContent, int statusCode)
        {
            sendResponse(charEncoder.GetBytes(JsonConvert.SerializeObject(bContent)), statusCode, extensions["json"]);
        }
        /// <summary>
        /// Automatically converts the dictionaries to json format
        /// </summary>
        /// <param name="bContent">dictionarys</param>
        /// <param name="statusCode">Answer status code</param>
        public void sendJson(List<Dictionary<string, string>> bContent, int statusCode)
        {
            sendResponse(charEncoder.GetBytes(JsonConvert.SerializeObject(bContent)), statusCode, extensions["json"]);
        }
        /// <summary>
        /// Automatically converts the object to json format
        /// </summary>
        /// <param name="bContent">dictionarys</param>
        /// <param name="statusCode">Answer status code</param>
        public void sendJson<T>(T bContent, int statusCode)
        {
            sendResponse(charEncoder.GetBytes(JsonConvert.SerializeObject(bContent)), statusCode, extensions["json"]);
        }
        /// <summary>
        /// set security Header (Disable xss)
        /// </summary>
        public void setSecurityHeader()
        {
            header["X-XSS-Protection"] = "1; mode=block";
        }
        /// <summary>
        /// To prevent xss attacks
        /// </summary>
        /// <param name="response">response</param>
        /// <returns></returns>
        public string safeResponse(string response)
        {
            return HttpUtility.HtmlEncode(response);
        }
        /// <summary>
        /// Redirect to path
        /// </summary>
        /// <param name="path">path or URL</param>
        public void redirect(string path)
        {
            header["Location"] = path;
            sendResponse(charEncoder.GetBytes(string.Empty), 308, string.Empty);
        }
    }
}
