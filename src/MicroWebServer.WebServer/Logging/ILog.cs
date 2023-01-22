using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroWebServer.WebServer.Logging
{
    interface ILog
    {
        void Informational(string Message);
        void Warning(string Message);
        void Alert(string Message);
        void Error(string Message);
        void Critical(string Message);
        void Debug(string Message);
    }
}
