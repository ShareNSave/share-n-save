using N2;

namespace ZeroWaste.SharePortal.Controllers
{
    /// <summary>
    /// Templates controller with error email handler
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class N2Controller<T> : TemplatesControllerBase<T> where T : ContentItem
    {
    }
}