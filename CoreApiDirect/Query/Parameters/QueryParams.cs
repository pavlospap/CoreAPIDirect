using System.Collections.Generic;

namespace CoreApiDirect.Query.Parameters
{
    /// <summary>
    /// Represents the query parameters.
    /// </summary>
    public class QueryParams
    {
        /// <summary>
        /// Gets or sets the search key.
        /// </summary>
        public string Search { get; set; }

        /// <summary>
        /// Gets or sets the selected fields.
        /// </summary>
        public IEnumerable<string> Fields { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the sort rules.
        /// </summary>
        public IEnumerable<QuerySort> Sort { get; set; } = new List<QuerySort>();

        /// <summary>
        /// Gets or sets the filters.
        /// </summary>
        public IEnumerable<QueryLogicalFilter> Filter { get; set; } = new List<QueryLogicalFilter>();
    }
}
