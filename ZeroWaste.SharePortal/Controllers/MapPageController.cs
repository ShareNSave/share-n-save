using System.Web.Mvc;
using ZeroWaste.SharePortal.Models.Parts;
using N2.Web;
using ZeroWaste.SharePortal.Models;

namespace ZeroWaste.SharePortal.Controllers
{
    [Controls(typeof(MapPage))]
    public class MapPageController : DBController<MapPage>
    {

        public override ActionResult Index()
        {
            MapPageModel model = new MapPageModel();
            return View(model);
        }
    }
}