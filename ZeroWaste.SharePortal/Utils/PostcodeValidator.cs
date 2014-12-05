using System.Text.RegularExpressions;

namespace ZeroWaste.SharePortal.Utils
{
    public static class PostcodeValidator
    {
        /// <summary>
        /// Regular expression for an australian postcode.
        /// </summary>
        /// <remarks>
        /// Australian postal code verification. Australia has 4-digit numeric postal codes with the following state  based specific ranges. 
        /// ACT: 0200-0299 and 2600-2639. 
        /// NSW: 1000-1999, 2000-2599 and 2640-2914.
        /// NT: 0900-0999 and 0800-0899. 
        /// QLD: 9000-9999 and 4000-4999. 
        /// SA: 5000-5999. 
        /// TAS: 7800-7999 and 7000-7499. 
        /// VIC: 8000-8999 and 3000-3999. 
        /// WA: 6800-6999 and 6000-6799
        /// 
        /// Pass:	0200|||7312|||2415
        /// Fail:	0300|||7612|||2915
        /// 
        /// From: http://www.dbsoftlab.com/regular-expressions/is-australian-post-code.html
        /// </remarks>
        private static readonly Regex AustralianPostcodeRegex = new Regex("^(0[289][0-9]{2})|([1345689][0-9]{3})|(2[0-8][0-9]{2})|(290[0-9])|(291[0-4])|(7[0-4][0-9]{2})|(7[8-9][0-9]{2})$", RegexOptions.Compiled);

        /// <summary>
        /// Returns a value indicating whether the <paramref name="poscode"/> parameter is a valid australian postcode.
        /// </summary>
        /// <param name="poscode">The postcode value to validate.</param>
        /// <returns><c>true</c> if the postcode is valid, otherwise <c>false</c>.</returns>
        public static bool IsValidAustralianPostcode(string poscode)
        {
            return !string.IsNullOrWhiteSpace(poscode) && AustralianPostcodeRegex.IsMatch(poscode);
        }
    }
}