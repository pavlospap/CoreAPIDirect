using CoreApiDirect.Url.Encoding;

namespace CoreApiDirect.Query.Operators
{
    /// <summary>
    /// A logical operator.
    /// </summary>
    public enum LogicalOperator
    {
        /// <summary>
        /// Performs a logical AND operation on two sub-expressions.
        /// </summary>
        [Encoded(Encoded.AND)]
        And,

        /// <summary>
        /// Performs a logical OR operation on two sub-expressions.
        /// </summary>
        [Encoded(Encoded.OR)]
        Or
    };
}
