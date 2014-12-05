using N2.Engine;

namespace ZeroWaste.SharePortal.Services
{
    [Service]
    [Service(typeof(ISharePortalSettings))]
    public class SharePortalSettings : ISharePortalSettings
    {
        private readonly int _defaultMaxListingResults = 18;

        public SharePortalSettings()
        {
            string number = System.Configuration.ConfigurationManager.AppSettings["MaxListingResult"];
            if (!string.IsNullOrWhiteSpace(number))
            {
                try
                {
                    int.TryParse(number, out _defaultMaxListingResults);
                }
                catch { }
            }
        }

        public int DefaultMaxListingResults
        {
            get { return _defaultMaxListingResults; }
        }
    }
}