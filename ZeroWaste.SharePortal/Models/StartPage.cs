using N2;
using N2.Definitions;
using N2.Details;
using N2.Engine.Globalization;
using N2.Web;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using N2.Web.UI;
using System.Web.UI.WebControls;

namespace ZeroWaste.SharePortal.Models
{
    /// <summary>
    /// This is the start page on a site. Separate start pages can respond to 
    /// a domain name and/or form the root of translation. The registration of
    /// this model is performed by <see cref="Registrations.StartPageRegistration"/>.
    /// </summary>
    /// 
    [TabContainer(AccountLinks, "Forget password", 10000)]
    [TabContainer(EmailTab, "Approval notification email", 20000)]
    public class StartPage : ContentPage, IStartPage, ILanguage, ISitesSource
    {
        #region ILanguage Members

        public virtual string LanguageCode { get; set; }

        public string LanguageTitle
        {
            get
            {
                if (string.IsNullOrEmpty(LanguageCode))
                    return "";
                else
                    return new CultureInfo(LanguageCode).DisplayName;
            }
        }

        #endregion
        private const string AccountLinks = "AccountLinks";
        private const string EmailTab = "ApprovalNotificationEmail";

        [EditableUrl("About us Page", 78, HelpText = "Page to display from the header about link.")]
        public virtual string AboutUsUrl { get; set; }

        [EditableUrl("Contact us Page", 79, HelpText = "Page to display the contact us page.")]
        public virtual string ContactUsPage { get; set; }

        [EditableUrl("Useful links Page", 80, HelpText = "Page to display the useful links.")]
        public virtual string UsefulLinksPage { get; set; }

        [EditableText(Title = "Gootle Analytics Key", ContainerName = Defaults.Containers.Site, HelpText = "Sets the google analytics key to use for the site.")]
        public virtual string GoogleAnalyticsKey
        {
            get { return GetDetail(Defaults.StartPageDetails.GoogleAnalyticsKey, string.Empty); }
            set { SetDetail(Defaults.StartPageDetails.GoogleAnalyticsKey, value, string.Empty); }
        }

        [EditableText(Title = "Gootle Analytics Domain", ContainerName = Defaults.Containers.Site, HelpText = "Sets the google analytics domain to use for the site.")]
        public virtual string GoogleAnalyticsDomain
        {
            get { return GetDetail(Defaults.StartPageDetails.GoogleAnalyticsDomain, string.Empty); }
            set { SetDetail(Defaults.StartPageDetails.GoogleAnalyticsDomain, value, string.Empty); }
        }

        public virtual string FooterText { get; set; }

        public virtual string Logotype { get; set; }

        [EditableUrl("Login Page", 79, HelpText = "Page to display when authorization to a page fails.")]
        public virtual string LoginPage
        {
            get { return (string)GetDetail("LoginPage"); }
            set { SetDetail("LoginPage", value); }
        }

        [EditableUrl("Send reset password email success page", 10, ContainerName = AccountLinks)]
        public virtual string SendResetPasswordEmailSuccessPage { get; set; }

        [EditableUrl("Reset password success page", 20, ContainerName = AccountLinks)]
        public virtual string ResetPasswordSuccessPage { get; set; }

        [EditableFreeTextArea("Reset password email content", 30, ContainerName = AccountLinks)]
        public virtual string ResetPasswordEmailContent { get; set; }

        [EditableTextBox("Mail from", 10, ContainerName = EmailTab)]
        public virtual string EmailMailFrom { get; set; }

        //[EditableTextBox("Mail to", 20, ContainerName = EmailTab)]
        //public virtual string EmailMailTo { get; set; }

        [EditableTextBox("Mail subject", 30, ContainerName = EmailTab)]
        public virtual string EmailMailSubject { get; set; }

        [EditableFreeTextArea("Mail intro", 40, ContainerName = EmailTab)]
        public virtual string EmailMailIntro { get; set; }

        #region ISitesSource Members

        public virtual string HostName { get; set; }

        public IEnumerable<Site> GetSites()
        {
            if (!string.IsNullOrEmpty(HostName))
                yield return new Site(Find.EnumerateParents(this, null, true).Last().ID, ID, HostName) { Wildcards = true };
        }

        #endregion
    }
}