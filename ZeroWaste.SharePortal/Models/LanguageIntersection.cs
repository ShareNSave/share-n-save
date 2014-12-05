﻿using System.Collections.Generic;
using System.Linq;
using N2;
using N2.Definitions;
using N2.Details;
using N2.Installation;
using N2.Integrity;
using N2.Security;
using N2.Web;
using N2.Web.UI;

namespace ZeroWaste.SharePortal.Models
{
	/// <summary>
	/// Redirects to the child start page that matches the user agent's language.
	/// </summary>
	[PageDefinition(IconClass = "n2-icon-globe n2-gold")]
	[RestrictParents(typeof(IRootPage))]
	[TabContainer(Defaults.Containers.Site, "Languages", 1000, ContainerName = "LanguagesContainer")]
	[RecursiveContainer("LanguagesContainer", 1000, RequiredPermission = Permission.Administer)]
	public class LanguageIntersection : PageModelBase, ISitesSource, IRedirect
	{
		#region ISitesSource Members

		[EditableText(Title = "Site collection host name (DNS)", 
			ContainerName = Defaults.Containers.Site,
			HelpTitle = "Sets a shared host name for all languages on a site. The web server must be configured to accept this host name for this to work.")]
		public virtual string HostName { get; set; }

		public IEnumerable<Site> GetSites()
		{
			if (!string.IsNullOrEmpty(HostName))
				yield return new Site(Find.EnumerateParents(this, null, true).Last().ID, ID, HostName) { Wildcards = true };
		}

		#endregion

		#region IRedirect Members

		public string RedirectUrl
		{
			get { return Children.OfType<StartPage>().Select(sp => sp.Url).FirstOrDefault() ?? this.Url; }
		}

		public ContentItem RedirectTo
		{
			get { return Children.OfType<StartPage>().FirstOrDefault(); }
		}

		#endregion
	}
}