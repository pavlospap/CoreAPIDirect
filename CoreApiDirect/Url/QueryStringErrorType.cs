namespace CoreApiDirect.Url
{
    /// <summary>
    /// Specifies the type of the query string error.
    /// </summary>
    public enum QueryStringErrorType
    {
        /// <summary>
        /// The parameter is invalid.
        /// </summary>
        InvalidParameter,

        /// <summary>
        /// The parameter format is invalid.
        /// </summary>
        InvalidFormat,

        /// <summary>
        /// The parameter contains invalid fields.
        /// </summary>
        InvalidField
    }
}
