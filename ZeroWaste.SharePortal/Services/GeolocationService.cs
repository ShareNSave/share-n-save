using System;
using System.Collections.Generic;
using System.Linq;
using GoogleMapsApi;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;
using N2.Engine;

namespace ZeroWaste.SharePortal.Services
{
    [Service]
    [Service(typeof (IGeoLocationService))]
    public class GeoLocationService : IGeoLocationService
    {
        /// <summary>
        /// Location cache to reduce the need for constantly geolocating postcodes.
        /// </summary>
        private readonly Dictionary<string, Location> LocationsCache = new Dictionary<string, Location>();

        public Location? GetCentroid(string postcode)
        {
            Location? location = null;
            if (LocationsCache.ContainsKey(postcode))
            {
                location = LocationsCache[postcode];
            }
            else
            {
                string targetAddress = string.Format("{0} Australia", postcode);

                try
                {
                    var geocodeRequest = new GeocodingRequest
                    {
                        Sensor = false,
                        Address = targetAddress,
                    };

                    var geocode = GoogleMaps.Geocode.Query(geocodeRequest);

                    if (geocode.Status == Status.OK)
                    {
                        if (geocode.Results != null)
                        {
                            var result = geocode.Results.FirstOrDefault();

                            if (result != null)
                            {
                                var l = Location.Create(result.Geometry.Location.Longitude, result.Geometry.Location.Latitude);

                                // Cache the location.
                                LocationsCache[postcode] = l;
                                location = l;
                            }
                        }
                    }
                }
                catch
                {
                }
            }

            return location;
        }
    }

    public struct Location : IEquatable<Location>
    {
        public static Location Create(double longitude, double latitude)
        {
            var location = new Location {Longitude = longitude, Latitude = latitude};
            return location;
        }

        public double Latitude { get; private set; }

        public double Longitude { get; private set; }

        public bool Equals(Location other)
        {
            return Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Location && Equals((Location) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Latitude.GetHashCode()*397) ^ Longitude.GetHashCode();
            }
        }

        public static bool operator ==(Location left, Location right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Location left, Location right)
        {
            return !left.Equals(right);
        }
    }

}