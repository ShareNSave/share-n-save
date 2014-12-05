using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using N2;
using N2.Definitions;
using N2.Details;
using N2.Integrity;
using N2.Persistence;

namespace ZeroWaste.SharePortal.Models
{
    [PageDefinition("News", Description = "A news page.", SortOrder = 155,
        IconClass = "n2-icon-file blue")]
    [RestrictParents(typeof(NewsContainer))]
    public class News : ContentPageBase, ISyndicatable
    {
        public News()
        {
            Visible = false;
            Syndicate = true;
        }
        
        [Persistable(PersistAs = PropertyPersistenceLocation.Detail)]
        public virtual bool Syndicate { get; set; }

        [EditableTags(SortOrder = 200)]
        public virtual IEnumerable<string> Tags { get; set; }
    }
}