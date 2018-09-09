using CoreApiDirect.Url.Encoding;

namespace CoreApiDirect.Query.Operators
{
    /// <summary>
    /// Specifies the sorting direction.
    /// </summary>
    public enum SortDirection
    {
        /// <summary>
        /// Sorts in ascending order.
        /// </summary>
        [Encoded(Encoded.ASC)]
        Ascending,

        /// <summary>
        /// Sorts in descending order.
        /// </summary>
        [Encoded(Encoded.DESC)]
        Descending
    };
}
