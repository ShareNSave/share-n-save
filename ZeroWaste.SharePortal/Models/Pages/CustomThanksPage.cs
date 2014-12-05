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
    [PageDefinition("Custom thanks page")]
    public class CustomThanksPage : ContentPage
    {
        [EditableFreeTextArea("Centre text message", 10)]
        public virtual string CentreTextMessage { get; set; }
    }
}