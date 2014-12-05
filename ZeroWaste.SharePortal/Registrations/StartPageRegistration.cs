using ZeroWaste.SharePortal.Controllers;
using ZeroWaste.SharePortal.Models;
using N2.Definitions;
using N2.Definitions.Runtime;
using N2.Details;
using N2.Security;
using N2.Web.Mvc;

namespace ZeroWaste.SharePortal.Registrations
{
	[Registration]
	public class StartPageRegistration : FluentRegisterer<StartPage>
	{
		public override void RegisterDefinition(IContentRegistration<StartPage> register)
		{
			register.ControlledBy<StartPageController>();

			register.Page(title: "Start Page", description: "The topmost node of a site. This can be placed below a language intersection to also represent a language");
			register.IconClass("n2-icon-globe");
			register.Definition.Installer = global::N2.Installation.InstallerHint.PreferredStartPage;
			register.RestrictParents(typeof(IRootPage), typeof(LanguageIntersection));

			using (register.RecursiveContainer("SiteContainer", headingFormat: null).Allow(Permission.Administer).Begin())
			{
				using (register.TabContainer(Defaults.Containers.Site, "Site").Begin())
				{
					register.On(sp => sp.LanguageCode).Languages();
					register.On(sp => sp.FooterText).FreeText("Footer text")
						.WithTokens();
					register.On(sp => sp.Logotype).ImageUpload();
					register.On(sp => sp.HostName).Text("Site host name (DNS)")
						.Help("Sets a host name for this site/language. The web server must be configured to accept this host name for this to work.");

				}
			}

			using (register.WithinContainer(Defaults.Containers.Metadata))
			{
				register.RegisterEditable(new WithEditableTemplateSelectionAttribute());
			}
		}
	}
}