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
    [PageDefinition("Instruction page")]
    public class InstructionPage : ContentPage
    {
        [EditableUrl("Create listing page", 10)]
        public virtual string CreateListingPage { get; set; }

        [EditableUrl("Login page", 20)]
        public virtual string LoginPage { get; set; }

        [EditableUrl("Register page", 30)]
        public virtual string RegisterPage { get; set; }
    }
}