using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZeroWaste.SharePortal.Models;
using N2;
using N2.Engine;
using N2.Web.Parts;

namespace ZeroWaste.SharePortal.Services
{
    [Adapts(typeof(PageModelBase))]
    public class TemplatesPartsAdapter : PartsAdapter
    {
        public override System.Collections.Generic.IEnumerable<ContentItem> GetParts(ContentItem parentItem, string zoneName, string @interface)
        {
            var items = base.GetParts(parentItem, zoneName, @interface);
            ContentItem grandParentItem = parentItem;
            if (zoneName.StartsWith("Recursive") && grandParentItem is ContentPageBase && !(grandParentItem is StartPage))
            {
                if (!parentItem.VersionOf.HasValue)
                    items = items.Union(GetParts(parentItem.Parent, zoneName, @interface));
                else
                    items = items.Union(GetParts(parentItem.VersionOf.Parent, zoneName, @interface));
            }
            return items;
        }
    }
}