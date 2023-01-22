using System.Collections.Generic;

namespace MicroWebServer.WebServer.IO
{
    public class Requests
    {
        /// <summary>
        /// Details of the request received
        /// method
        /// path
        /// httpVersion
        /// </summary>
        public Dictionary<string, string> requestInfo { get; set; }
        /// <summary>
        /// Request headers received
        /// </summary>
        public Dictionary<string, string> header = new Dictionary<string, string>();
        /// <summary>
        /// Request cookies received
        /// </summary>
        public Dictionary<string, string> cookie = new Dictionary<string, string>();
        /// <summary>
        /// Data received on request (POST,PUT,OPTIONS)
        /// </summary>
        public string body { get; set; }
        public Requests(string request)
        {
            string[] Info = request.Split("\n")[0].Split(" ");
            requestInfo = new Dictionary<string, string>()
            {
                {"method" ,Info[0]},
                {"path" ,Info[1]},
                {"httpVersion",Info[2]}
            };
            Splitter(request);
        }
        private void Splitter(string request)
        {
            string[] headerAndBody = request.Split("\r\n\r\n");
            if (requestInfo["method"] != "GET" && requestInfo["method"] != "DELETE")
            {
                body = headerAndBody[1];
            }
            string[] dataSplited = headerAndBody[0].Split("\n");
            for (int i = 1; i < dataSplited.Length; i++)
            {
                if (dataSplited[i].Contains(":"))
                {
                    if (dataSplited[i].Split(":")[0] == "Cookie")
                    {
                        string[] item = dataSplited[i].Split(":");
                        if (item[1].Contains(";"))
                        {
                            item = item[1].Split(";");
                            foreach (var cookieItem in item)
                            {
                                cookie[cookieItem.Split("=")[0].Trim()] = cookieItem.Split("=")[1];
                            }
                        }
                        else
                        {
                            cookie[item[1].Split("=")[0].Trim()] = item[1].Split("=")[1];
                        }
                    }
                    else
                    {
                        string[] segment = dataSplited[i].Split(":");
                        header[segment[0].Trim().Trim()] = segment[1].Trim();
                    }
                }
            }

        }
        /// <summary>
        /// Get a header
        /// </summary>
        /// <param name="key">Header Key</param>
        /// <param name="defaultValue">Default Value</param>
        /// <returns></returns>
        public string getHeader(string key,string defaultValue)
        {
            return header.ContainsKey(key) ? header[key] : defaultValue;
        }
        /// <summary>
        /// Get a Cookie
        /// </summary>
        /// <param name="key">Cookie Key</param>
        /// <param name="defaultValue">Default Value</param>
        /// <returns></returns>
        public string getCookie(string key, string defaultValue)
        {
            return cookie.ContainsKey(key) ? cookie[key] : defaultValue;
        }
        /// <summary>
        /// Get URL arguments
        /// </summary>
        /// <param name="key">Argument Name</param>
        /// <param name="defaultValue">Default Value</param>
        /// <returns></returns>
        public string getArg(string key,string defaultValue)
        {
            if (requestInfo["path"].Contains('?'))
            {
                string allArgs = requestInfo["path"].Split('?')[1];
                if (allArgs.Contains('&'))
                {
                    string[] args = allArgs.Split('&');
                    foreach (var item in args)
                    {
                        if (item.Split('=')[0]==key)
                        {
                            return item.Split('=')[1];
                        }
                    }
                }
                if (allArgs.Split('=')[0] == key)
                {
                    return allArgs.Split('=')[1];
                }
            }
            return defaultValue;
        }
        /// <summary>
        /// Get a  Authorization Key
        /// </summary>
        /// <returns></returns>
        public Dictionary<string,string> getAuthHeader()
        {
            if (header.ContainsKey("Authorization"))
            {
                string[] authorizKey = header["Authorization"].Split(" ");
                switch (authorizKey[0])
                {
                    case "Token":
                        return new Dictionary<string, string>() { { "Token", authorizKey[1] } };
                    default:
                        break;
                }
            }
            return null;
        }
    }
}
