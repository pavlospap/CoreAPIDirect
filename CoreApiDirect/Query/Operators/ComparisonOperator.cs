using CoreApiDirect.Url.Encoding;

namespace CoreApiDirect.Query.Operators
{
    /// <summary>
    /// A comparison operator.
    /// </summary>
    public enum ComparisonOperator
    {
        /// <summary>
        /// The field is not equal to the filter value.
        /// </summary>
        [Encoded(Encoded.NOT_EQUAL)]
        NotEqual,

        /// <summary>
        /// The field is greater or equal to the filter value.
        /// </summary>
        [Encoded(Encoded.GREATER_OR_EQUAL)]
        GreaterOrEqual,

        /// <summary>
        /// The field is less or equal to the filter value.
        /// </summary>
        [Encoded(Encoded.LESS_OR_EQUAL)]
        LessOrEqual,

        /// <summary>
        /// The field is equal to the filter value.
        /// </summary>
        [Encoded(Encoded.EQUAL)]
        Equal,

        /// <summary>
        /// The field is greater than the filter value.
        /// </summary>
        [Encoded(Encoded.GREATER)]
        Greater,

        /// <summary>
        /// The field is less than the filter value.
        /// </summary>
        [Encoded(Encoded.LESS)]
        Less,

        /// <summary>
        /// The field does not exist in a list of values.
        /// </summary>
        [Encoded(Encoded.NOT_IN)]
        NotIn,

        /// <summary>
        /// The field exists in a list of values.
        /// </summary>
        [Encoded(Encoded.IN)]
        In,

        /// <summary>
        /// The field is not null.
        /// </summary>
        [Encoded(Encoded.NOT_NULL)]
        NotNull,

        /// <summary>
        /// The field is null.
        /// </summary>
        [Encoded(Encoded.NULL)]
        Null,

        /// <summary>
        /// The field is not like the filter value.
        /// </summary>
        [Encoded(Encoded.NOT_LIKE)]
        NotLike,

        /// <summary>
        /// The field is like the filter value.
        /// </summary>
        [Encoded(Encoded.LIKE)]
        Like
    }
}
