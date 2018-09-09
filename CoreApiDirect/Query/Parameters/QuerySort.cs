using CoreApiDirect.Query.Operators;

namespace CoreApiDirect.Query.Parameters
{
    /// <summary>
    /// Represents a query sort rule.
    /// </summary>
    public class QuerySort
    {
        /// <summary>
        /// Gets or sets the field name.
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Gets or sets the sort direction.
        /// </summary>
        public SortDirection Direction { get; set; }
    }
}
