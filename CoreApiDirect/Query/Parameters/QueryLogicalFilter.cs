using CoreApiDirect.Query.Operators;

namespace CoreApiDirect.Query.Parameters
{
    /// <summary>
    /// Represents a query logical filter.
    /// </summary>
    public class QueryLogicalFilter
    {
        /// <summary>
        /// Gets or sets the logical operator.
        /// </summary>
        public LogicalOperator Operator { get; set; }

        /// <summary>
        /// Gets or sets the comparison filter.
        /// </summary>
        public QueryComparisonFilter Filter { get; set; }
    }
}
