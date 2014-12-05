using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using N2;
using N2.Details;
using N2.Web.UI;
using NHibernate.Hql.Ast.ANTLR;

namespace ZeroWaste.SharePortal.Models.Pages
{
    [PageDefinition("Contact us page")]
    [TabContainer(EmailTab, EmailTab, 1000)]
    [TabContainer(ReplyEmailTab, ReplyEmailTab, 2000)]
    public class ContactUsPage : ContentPage
    {
        private const string EmailTab = "Email";
        private const string ReplyEmailTab = "ReplyEmail";

        [EditableUrl("Thank you page", 10)]
        public virtual string ThankYouPage { get; set; }

        [EditableTextBox("Mail from", 10, ContainerName = EmailTab)]
        public virtual string EmailMailFrom { get; set; }

        [EditableTextBox("Mail to", 20, ContainerName = EmailTab)]
        public virtual string EmailMailTo { get; set; }

        [EditableTextBox("Mail subject", 30, ContainerName = EmailTab)]
        public virtual string EmailMailSubject { get; set; }

        [EditableFreeTextArea("Mail intro", 40, ContainerName = EmailTab)]
        public virtual string EmailMailIntro { get; set; }

        [EditableTextBox("Mail subject", 10, ContainerName = ReplyEmailTab)]
        public virtual string ReplyEmailMailSubject { get; set; }

        [EditableFreeTextArea("Mail intro", 20, ContainerName = ReplyEmailTab)]
        public virtual string ReplyEmailMailIntro { get; set; }
    }
}