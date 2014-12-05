using System.Web.Mvc;
using ZeroWaste.SharePortal.Models.Data;

namespace ZeroWaste.SharePortal.Controllers
{
    public abstract class MvcDBController : Controller
    {
        private ZeroWasteData _dataContext;

        public ZeroWasteData DataContext
        {
            get
            {
                if (_dataContext == null)
                {
                    _dataContext = new ZeroWasteData();
                }
                return _dataContext;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_dataContext != null)
            {
                _dataContext.Dispose();
                _dataContext = null;
            }
            base.Dispose(disposing);
        }
    }
}
