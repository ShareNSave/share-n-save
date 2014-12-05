using N2.Definitions;
using N2.Details;
using N2.Persistence;

namespace ZeroWaste.SharePortal.Models
{
    public abstract class ContentPageBase : PageModelBase, IContentPage
    {
        /// <summary>
        /// Summary text displayed in listings.
        /// </summary>
        [EditableSummary(Title = "Summary", SortOrder = 200, Source = "Text", ContainerName = Defaults.Containers.Content)]
        [Persistable(Length = 1024)] // to minimize select+1
        public virtual string Summary { get; set; }

        /// <summary>
        /// Main content of this content item.
        /// </summary>
        [EditableFreeTextArea(SortOrder = 201, ContainerName = Defaults.Containers.Content)]
        [DisplayableTokens]
        public virtual string Text { get; set; }
        
    }
}