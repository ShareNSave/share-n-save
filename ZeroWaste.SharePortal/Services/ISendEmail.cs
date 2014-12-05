using System.Net.Mail;
namespace ZeroWaste.SharePortal.Services
{
    public interface ISendEmail
    {
        void SendingEmail(MailMessage message);
    }
}