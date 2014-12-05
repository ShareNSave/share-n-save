using System.Web.Mvc;
using ZeroWaste.SharePortal.Models;
using N2.Web;
using N2.Web.Mvc;

namespace ZeroWaste.SharePortal.Controllers
{
    [Controls(typeof(ContentPage))]
	public class ContentPagesController : ContentController<ContentPage>
	{
		public override ActionResult Index()
		{
			return View(CurrentItem.TemplateKey, CurrentItem);
		}
	}
}
