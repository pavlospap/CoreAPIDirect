using System.Collections.Generic;
using CoreApiDirect.Query.Operators;

namespace CoreApiDirect.Query.Parameters
{
    /// <summary>
    /// Represents a query comparison filter.
    /// </summary>
    public class QueryComparisonFilter
    {
        /// <summary>
        /// Gets or sets the field name.
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Gets or sets the comparison operator.
        /// </summary>
        public ComparisonOperator Operator { get; set; }

        /// <summary>
        /// Gets or sets an enumerable of filter values.
        /// </summary>
        public IEnumerable<string> Values { get; set; } = new List<string>();
    }
}
