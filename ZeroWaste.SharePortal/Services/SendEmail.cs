using System.Net.Mail;
using N2.Engine;
namespace ZeroWaste.SharePortal.Services
{
    [Service]
    [Service(typeof(ISendEmail))]
    public class SendEmail : ISendEmail
    {
        public void SendingEmail(MailMessage message)
        {
            SmtpClient client = new SmtpClient();
            client.Send(message);
        }
    }
}