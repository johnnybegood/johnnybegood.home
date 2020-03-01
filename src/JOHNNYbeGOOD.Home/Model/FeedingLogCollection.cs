using System.Collections.Generic;
using System.Linq;

namespace JOHNNYbeGOOD.Home.Models
{
    /// <summary>
    /// Collection of <see cref="FeedingLog"/>
    /// </summary>
    public class FeedingLogCollection
    {
        /// <summary>
        /// Past feedings
        /// </summary>
        public IReadOnlyCollection<FeedingLog> Items { get; }

        /// <summary>
        /// Default constructor for <see cref="FeedingLogCollection"/>
        /// </summary>
        public FeedingLogCollection()
        {
            Items = new List<FeedingLog>();
        }

        /// <summary>
        /// Constructor for <see cref="FeedingLogCollection"/> with predefined list of items
        /// </summary>
        public FeedingLogCollection(IEnumerable<FeedingLog> items)
        {
            Items = items.ToList();
        }
    }
}