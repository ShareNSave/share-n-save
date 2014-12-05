using System.Web.Mvc;
using ZeroWaste.SharePortal.Models;
using N2.Web;
using N2.Web.Mvc;

namespace ZeroWaste.SharePortal.Controllers
{
    [Controls(typeof(LanguageIntersection))]
	public class LanguageIntersectionController : ContentController<LanguageIntersection>
	{
		public override ActionResult Index()
		{
		    if (CurrentItem == null)
		        return View();

			var language = Request.SelectLanguage(CurrentItem);
			if (language != null)
			{
				if (language.Url.StartsWith("http"))
					return Redirect(language.Url);

				return ViewPage(language);
			}

			if(CurrentItem.RedirectUrl != CurrentItem.Url)
				return Redirect(CurrentItem.RedirectUrl);

			return View(CurrentItem);
		}

	}
}
