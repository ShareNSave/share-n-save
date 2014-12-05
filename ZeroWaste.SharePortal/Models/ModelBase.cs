using System.Collections.Generic;
using N2;
using N2.Collections;
using N2.Web.UI;

namespace ZeroWaste.SharePortal.Models
{
    [SidebarContainer(Defaults.Containers.Metadata, 100, HeadingText = "Metadatda")]
    public class ModelBase : ContentItem
    {
        /// <summary>
        /// Get children where type of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual IList<T> GetChildren<T>() where T : ContentItem
        {
            return new ItemList<T>(Children,
                new AccessFilter(),
                new TypeFilter(typeof(T)));
        }

        /// <summary>
        /// Get children where type of T and in zone
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="zoneName"></param>
        /// <returns></returns>
        public virtual IList<T> GetChildren<T>(string zoneName) where T : ContentItem
        {
            return new ItemList<T>(Children,
                new AccessFilter(),
                new TypeFilter(typeof(T)),
                new ZoneFilter(zoneName));
        }

        /// <summary>
        /// The view name to use (for MVC). Defaults to the "Type.Name".
        /// </summary>
        public virtual string ViewTemplate
        {
            get { return GetContentType().Name; }
        }
    }
}