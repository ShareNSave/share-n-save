using System.Web.UI.WebControls;
using N2;
using N2.Definitions;
using N2.Details;
using N2.Integrity;
using N2.Persistence;

namespace ZeroWaste.SharePortal.Models
{
    /// <summary>
    /// Content page model used in several places:
    ///  * It serves as base class for start page
    ///  * It's the base for "template first" definitions located in /dinamico/default/views/contentpages/
    /// </summary>
    [PageDefinition]
    [WithEditableTemplateSelection(ContainerName = Defaults.Containers.Metadata)]
    public class ContentPage : ContentPageBase, IStructuralPage
    {
        /// <summary>
        /// Image used on the page and on listings.
        /// </summary>
        [EditableMediaUpload(PreferredSize = "wide")]
        [Persistable(Length = 256)] // to minimize select+1
        public virtual string Image { get; set; }

        /// <summary>
        /// Title that replaces the regular title when not empty.
        /// </summary>
        [EditableText(Title = "SEO Title", ContainerName = Defaults.Containers.Metadata)]
        public virtual string SeoTitle { get; set; }
    }
}