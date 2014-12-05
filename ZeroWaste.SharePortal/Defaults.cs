using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZeroWaste.SharePortal.Models;
using N2;
using N2.Definitions;

namespace ZeroWaste.SharePortal
{
	public static class Defaults
    {
        public static readonly List<string> ContainerWrappableZones = new List<string> { "BeforeMain", "BeforeMainRecursive", "BeforeMainSite", "AfterMain", "AfterMainRecursive", "AfterMainSite" };

		public static class Containers
		{
			public const string Metadata = "Metadata";
			public const string Content = "Content";
			public const string Site = "Site";
			public const string Advanced = "Advanced";
		}

        public static class StartPageDetails
        {
            public const string GoogleAnalyticsKey = "GoogleAnalyticsKey";
            public const string GoogleAnalyticsDomain = "GoogleAnalyticsDomain";
        }

        /// <summary>
        /// Provides access to some zone-related constants. Use these constants 
        /// instead of strings for better compile-time checking.
        /// </summary>
        public static class Zones
        {
            /// <summary>Right column on whole site.</summary>
            public const string SiteRight = "SiteRight";
            /// <summary>Above content on whole site.</summary>
            public const string SiteTop = "SiteTop";
            /// <summary>Left of content on whole site.</summary>
            public const string SiteLeft = "SiteLeft";

            /// <summary>To the right on this and child pages.</summary>
            public const string RecursiveRight = "RecursiveRight";
            /// <summary>To the left on this and child pages.</summary>
            public const string RecursiveLeft = "RecursiveLeft";
            /// <summary>Above the content area on this and child pages.</summary>
            public const string RecursiveAbove = "RecursiveAbove";
            /// <summary>Below the content area on this and child pages.</summary>
            public const string RecursiveBelow = "RecursiveBelow";

            /// <summary>Right on this page.</summary>
            public const string Right = "Right";
            /// <summary>Left on this page</summary>
            public const string Left = "Left";

            /// <summary>On the left side of a two column container</summary>
            public const string ColumnLeft = "ColumnLeft";
            /// <summary>On the right side of a two column container</summary>
            public const string ColumnRight = "ColumnRight";

            /// <summary>In the content column (below text) on this page.</summary>
            public const string Content = "Content";

            /// <summary> Questions on FAQ pages </summary>
            public const string Questions = "Questions";
        }
		
		public static string ImageSize(string preferredSize, string fallbackToZoneNamed)
		{
			if (string.IsNullOrEmpty(preferredSize))
				return ImageSize(fallbackToZoneNamed);
			return preferredSize;
		}

		public static string ImageSize(string zoneName)
		{
			switch (zoneName)
			{
				case "SliderArea":
				case "PreContent":
				case "PostContent":
					return "wide";
				default:
					return "half";
			}
		}

        public static bool IsContainerWrappable(string zoneName)
        {
            if (string.IsNullOrEmpty(zoneName))
                return false;

            if (ContainerWrappableZones.Contains(zoneName))
                return true;

            return false;
        }


		/// <summary>
		/// Picks the translation best matching the browser-language or the first translation in the list
		/// </summary>
		/// <param name="request"></param>
		/// <param name="currentPage"></param>
		/// <returns></returns>
		public static ContentItem SelectLanguage(this HttpRequestBase request, ContentItem currentPage)
		{
			var start = Find.ClosestOf<IStartPage>(currentPage) ?? Find.StartPage;
			if (start == null) return null;

			if (start is LanguageIntersection)
			{
				var translations = GetTranslations(currentPage).ToList();

				if (request.UserLanguages == null)
					return translations.FirstOrDefault();

				var selectedlanguage = request.UserLanguages.Select(ul => translations.FirstOrDefault(t => t.LanguageCode == ul)).FirstOrDefault(t => t != null);
				return selectedlanguage ?? translations.FirstOrDefault();
			}

			return start;
		}

		private static IEnumerable<StartPage> GetTranslations(ContentItem currentPage)
		{
            if (currentPage == null)
                return new StartPage[0];
			return currentPage.GetChildren().OfType<StartPage>();
		}
	}
}