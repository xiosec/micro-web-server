using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
namespace MicroWebServer.WebServer.Utility
{
   public class HttpUtilitys
    {
        public string HtmlEncode(string value)
        {
            return HttpUtility.HtmlEncode(value);
        }
        public string HtmlDecode(string value)
        {
            return HttpUtility.UrlDecode(value);
        }
        public string UrlEncode(string value)
        {
            return HttpUtility.UrlEncode(value);
        }
        public string UrlDecode(string value)
        {
            return HttpUtility.UrlDecode(value);
        }
    }
}
