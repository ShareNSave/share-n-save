using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ZeroWaste.SharePortal.Models;
using ZeroWaste.SharePortal.Models.Data;
using ZeroWaste.SharePortal.Models.Pages;
using ZeroWaste.SharePortal.Services;
using N2;
using N2.Web;
using GoogleMapsApi;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;
using System.Data.Entity.Spatial;
using System.Web.Security;
using System.Text;
using N2.Web.Mail;
using System.Net.Mail;

namespace ZeroWaste.SharePortal.Controllers
{
    [Controls(typeof(ContactUsPage))]
    public class ContactUsPageController : DBController<ContactUsPage>
    {
        private readonly IMailSender _mailSender;
        public ContactUsPageController(IMailSender mailSender)
        {
            _mailSender = mailSender;
        }

        public override ActionResult Index()
        {
            return View(new ContactUsPageModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ContactUsPageModel model)
        {
            if (ModelState.IsValid)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("{0}: {1}<br/>", "Name", model.Name);
                sb.AppendFormat("{0}: {1}<br/>", "Email", model.Email);
                sb.AppendFormat("{0}: {1}<br/>", "Orgaisation or Group", model.OrganisationOrGroup);
                
                sb.AppendFormat("{0}: {1}<br/>", "Street or Postal Address", model.Street);
                sb.AppendFormat("{0}: {1}<br/>", "City", model.City);
                sb.AppendFormat("{0}: {1}<br/>", "State", model.State);
                sb.AppendFormat("{0}: {1}<br/>", "Postcode", model.Postcode);
                sb.AppendFormat("{0}: {1}<br/>", "Phone", model.PhoneNumber);
                sb.AppendFormat("{0}: {1}<br/>", "Message", model.Message);

                var contactUsPage = N2.Find.CurrentPage as ContactUsPage;
                if (contactUsPage != null)
                {
                    MailMessage mm = new MailMessage(contactUsPage.EmailMailFrom, contactUsPage.EmailMailTo);
                    mm.Subject = contactUsPage.EmailMailSubject;
                    mm.Body = sb.ToString();
                    mm.IsBodyHtml = true;

                    try
                    {
                        _mailSender.Send(mm);
                    }
                    catch
                    {
                        try
                        {
                            _mailSender.Send(mm);
                        }
                        catch { }
                    }
                }
                sb.Clear();
                sb = null;

                var clientEmailContent = contactUsPage.ReplyEmailMailIntro;
                if (!string.IsNullOrWhiteSpace(clientEmailContent))
                {
                    clientEmailContent = clientEmailContent.Replace("[name]", model.Name);
                    MailMessage mm = new MailMessage(contactUsPage.EmailMailFrom, model.Email);
                    mm.Subject = contactUsPage.ReplyEmailMailSubject;
                    mm.Body = clientEmailContent;
                    mm.IsBodyHtml = true;

                    try
                    {
                        _mailSender.Send(mm);
                    }
                    catch
                    {
                        try
                        {
                            _mailSender.Send(mm);
                        }
                        catch { }
                    }
                }

                if (!string.IsNullOrWhiteSpace(contactUsPage.ThankYouPage))
                {
                    return Redirect(contactUsPage.ThankYouPage);
                }
            }
            return View(model);
        }
    }
}