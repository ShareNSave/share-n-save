namespace ZeroWaste.SharePortal.Services
{
    public interface IGeoLocationService
    {
        Location? GetCentroid(string postcode);
    }
}