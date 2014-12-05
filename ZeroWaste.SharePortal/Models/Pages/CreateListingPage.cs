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
    [PageDefinition]
    [TabContainer(EmailTab, EmailTab, 10000)]
    public class CreateListingPage : PageModelBase
    {
        private const string EmailTab = "Email";

        /// <summary>
        /// Main content of this content item.
        /// </summary>
        [EditableFreeTextArea(SortOrder = 201, ContainerName = Defaults.Containers.Content)]
        [DisplayableTokens]
        public virtual string Text { get; set; }

        [EditableUrl("Listing success page", 79, HelpText = "Page to display when a listing has been successfully created.")]
        public virtual string SuccessfulListingPage { get; set; }

        [EditableFreeTextArea("Head text for admin", 210)]
        public virtual string HeadTextForAdmin { get; set; }

        [EditableFreeTextArea("Head text for public", 220)]
        public virtual string HeadTextForPublic { get; set; }

        [EditableTextBox("Mail from", 10, ContainerName = EmailTab)]
        public virtual string EmailMailFrom { get; set; }

        [EditableTextBox("Mail to admin", 20, ContainerName = EmailTab)]
        public virtual string EmailMailTo { get; set; }

        [EditableTextBox("Mail subject", 30, ContainerName = EmailTab)]
        public virtual string EmailMailSubject { get; set; }

        [EditableFreeTextArea("Mail intro for admin", 40, ContainerName = EmailTab)]
        public virtual string EmailMailIntro { get; set; }

        [EditableFreeTextArea("Mail intro for public", 50, ContainerName = EmailTab)]
        public virtual string EmailMailIntroPublic { get; set; }
    }
}