using ZeroWaste.SharePortal.Models;
using N2.Web;

namespace ZeroWaste.SharePortal.Controllers
{
    /// <summary>
    /// This controller will handle parts deriving from AbstractItem which are not 
    /// defined by another controller [Controls(typeof(MyPart))]. The default 
    /// behavior is to render a template with this pattern:
    ///  * "~/Views/SharedParts/{ContentTypeName}.ascx"
    /// </summary>
    [Controls(typeof(PartModelBase))]
    public class SharedPartsController : N2Controller<PartModelBase>
    {
    }
}