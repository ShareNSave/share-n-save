using N2.Definitions;
using N2.Details;
using N2.Integrity;
using N2.Web.UI;

namespace ZeroWaste.SharePortal.Models
{
    /// <summary>
    /// Base implementation for pages on a dinamico site.
    /// </summary>
    [WithEditableTitle]
    [WithEditableName(ContainerName = Defaults.Containers.Metadata)]
    [WithEditableVisibility(ContainerName = Defaults.Containers.Metadata)]
    [TabContainer(Defaults.Containers.Content, "Content", 1000)]
    [RestrictParents(typeof(IPage))]
    [AvailableZone("Before main", "BeforeMain")]
    [AvailableZone("Before main recursive", "BeforeMainRecursive")]
    [AvailableZone("After main", "AfterMain")]
    [AvailableZone("After main recursive", "AfterMainRecursive")]
    [AvailableZone("Left", "Left")]
    [AvailableZone("Right", "Right")]
    public abstract class PageModelBase : ModelBase, IPage
    {
        [EditableCheckBox("Display title on page", 40, ContainerName = Defaults.Containers.Metadata, DefaultValue = true, HelpText = "Checking this option displays the page title by default on the page")]
        public virtual bool ShowTitle { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether this <see cref="ContentPage"/> is visible in the header links.
        /// </summary>
        [EditableCheckBox("Display in header links", 1, ContainerName = Defaults.Containers.Metadata, HelpTitle = "Checking this option displays the page in the header links")]
        public virtual bool VisibleHeader { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether this <see cref="ContentPage"/> is visible in the footer links.
        /// </summary>
        [EditableCheckBox("Display in footer links", 20, ContainerName = Defaults.Containers.Metadata, HelpTitle = "Checking this option displays the page in footer links")]
        public virtual bool VisibleFooter { get; set; }

        [EditableCheckBox("Display the postcode filter on this page", 30, ContainerName = Defaults.Containers.Metadata, HelpTitle = "Checking this option displays the postcode filter on this page")]
        public bool ShowPostcodeFilter
        {
            get { return GetDetail("ShowPostcodeFilter", false); }
            set { SetDetail("ShowPostcodeFilter", value, false); }
        }
    }
}