using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace ZeroWaste.SharePortal.Utils
{
    public static class ErrorLog
    {
        public static void AddLog(string url, string controller, string action, string msg)
        {
            try
            {
                var smpt = ConfigurationManager.AppSettings["errorlog-smtp"];
                var port = ConfigurationManager.AppSettings["errorlog-port"];
                var user = ConfigurationManager.AppSettings["errorlog-user"];
                var pwd = ConfigurationManager.AppSettings["errorlog-pwd"];
                var from = ConfigurationManager.AppSettings["errorlog-from"];
                var to = ConfigurationManager.AppSettings["errorlog-to"];
                var subject = ConfigurationManager.AppSettings["errorlog-subject"];

                if (!string.IsNullOrEmpty(smpt) &&
                    !string.IsNullOrEmpty(from) &&
                    !string.IsNullOrEmpty(to) &&
                    !string.IsNullOrEmpty(subject))
                {

                    var sb = new StringBuilder();
                    sb.AppendLine("Server time: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    var ip = HttpContext.Current.Request.UserHostAddress;
                    if (string.IsNullOrEmpty(ip))
                    {
                        try
                        {
                            ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                        }
                        catch { }
                    }
                    sb.AppendLine("IP address: " + ip);
                    sb.AppendLine("Url: " + url);
                    sb.AppendLine("Controller: " + controller);
                    sb.AppendLine("Action: " + action);
                    sb.AppendLine();
                    sb.AppendLine(msg);

                    var iport = 25;
                    if (!string.IsNullOrEmpty(port)) iport = Convert.ToInt32(port);

                    var message = new MailMessage(from, to, subject, sb.ToString());

                    var client = new SmtpClient(smpt, iport);
                    if (!string.IsNullOrEmpty(user))
                    {
                        client.UseDefaultCredentials = true;
                        client.Credentials = new NetworkCredential(user, pwd);
                    }
                    client.Send(message);
                }
            }
            catch { }
        }
    }
}