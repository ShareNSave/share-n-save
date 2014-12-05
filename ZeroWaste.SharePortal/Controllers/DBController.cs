using ZeroWaste.SharePortal.Models.Data;
using N2;

namespace ZeroWaste.SharePortal.Controllers
{
    public abstract class DBController<T> : N2Controller<T> where T : ContentItem
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
