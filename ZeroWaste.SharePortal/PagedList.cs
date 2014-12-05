using System.Collections.Generic;

namespace ZeroWaste.SharePortal
{
    /// <summary>
    /// A collection of items with page information
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedList<T> : List<T>
    {
        /// <summary>
        /// The total number of pages
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// The total number of individual items
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// The current page number (not zero-based)
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// The pagesize
        /// </summary>
        public int PageSize { get; set; }
    }
}