namespace ZeroWaste.SharePortal.Services
{
    public interface IExploreListingSettings
    {
        bool EnableExploreDistanceSearch { get; }
        double DefaultMaxListingDistances { get; }
    }
}