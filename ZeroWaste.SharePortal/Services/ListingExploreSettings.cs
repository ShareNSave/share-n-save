using N2.Engine;

namespace ZeroWaste.SharePortal.Services
{
    [Service]
    [Service(typeof(IExploreListingSettings))]
    public class ListingExploreSettings : IExploreListingSettings
    {
        private readonly double _distance = 3000;
        private readonly bool _enableExploreDistanceSearch = false;

        public ListingExploreSettings()
        {
            string number = System.Configuration.ConfigurationManager.AppSettings["distance"];
            if (!string.IsNullOrWhiteSpace(number))
            {
                try
                {
                    double.TryParse(number, out _distance);
                }
                catch { }
            }

            string enableDistanceSearch = System.Configuration.ConfigurationManager.AppSettings["EnableExploreDistanceSearch"];
            if (!string.IsNullOrWhiteSpace(enableDistanceSearch))
            {
                try
                {
                    bool.TryParse(enableDistanceSearch, out _enableExploreDistanceSearch);
                }
                catch { }
            }
        }

        public double DefaultMaxListingDistances
        {
            get { return _distance; }
        }

        public bool EnableExploreDistanceSearch
        {
            get { return _enableExploreDistanceSearch; }
        }
    }
}