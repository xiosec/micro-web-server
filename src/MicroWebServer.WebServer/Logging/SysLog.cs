using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Rebex.Net;

namespace MicroWebServer.WebServer.Logging
{
    public class SysLog:ILog
    {
        private SyslogClient client;
        private SyslogMessage sysLogMessage;
        public SysLog(string HOST,int PORT)
        {
            client = new SyslogClient(SyslogTransportProtocol.Tcp, HOST, PORT);
            sysLogMessage = new SyslogMessage();
            sysLogMessage.Facility = SyslogFacilityLevel.User;
        }

        public void Alert(string Message)
        {
            sysLogMessage.Severity = SyslogSeverityLevel.Alert;
            sysLogMessage.Text = Message;
            client.Send(sysLogMessage);
        }

        public void Critical(string Message)
        {
            sysLogMessage.Severity = SyslogSeverityLevel.Critical;
            sysLogMessage.Text = Message;
            client.Send(sysLogMessage);
        }

        public void Debug(string Message)
        {
            sysLogMessage.Severity = SyslogSeverityLevel.Debug;
            sysLogMessage.Text = Message;
            client.Send(sysLogMessage);
        }

        public void Error(string Message)
        {
            sysLogMessage.Severity = SyslogSeverityLevel.Error;
            sysLogMessage.Text = Message;
            client.Send(sysLogMessage);
        }

        public void Informational(string Message)
        {
            sysLogMessage.Severity = SyslogSeverityLevel.Informational;
            sysLogMessage.Text = Message;
            client.Send(sysLogMessage);
        }

        public void Warning(string Message)
        {
            sysLogMessage.Severity = SyslogSeverityLevel.Critical;
            sysLogMessage.Text = Message;
            client.Send(sysLogMessage);
        }
    }
}
