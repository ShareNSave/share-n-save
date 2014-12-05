using N2;
using N2.Details;

namespace ZeroWaste.SharePortal.Models
{
	/// <summary>
	/// This part model is the base of several "template first" definitions 
	/// located in /dinamico/default/views/contentparts/ 
	/// </summary>
	[PartDefinition]
    [WithEditableTitle]
	[WithEditableTemplateSelection(ContainerName = Defaults.Containers.Metadata)]
	public class ContentPart : PartModelBase
	{
	}
}