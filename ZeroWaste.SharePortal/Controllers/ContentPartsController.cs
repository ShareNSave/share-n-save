using System.Web.Mvc;
using ZeroWaste.SharePortal.Models;
using N2.Web;
using N2.Web.Mvc;

namespace ZeroWaste.SharePortal.Controllers
{
    [Controls(typeof(ContentPart))]
	public class ContentPartsController : ContentController<ContentPart>
	{
		public override ActionResult Index()
		{
			return PartialView(CurrentItem.TemplateKey, CurrentItem);
		}
	}
}
