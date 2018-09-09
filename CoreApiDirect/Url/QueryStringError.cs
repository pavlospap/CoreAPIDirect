namespace CoreApiDirect.Url
{
    /// <summary>
    /// Represents an error in the URL query string.
    /// </summary>
    public class QueryStringError
    {
        /// <summary>
        /// Gets or sets the type of the error.
        /// </summary>
        public QueryStringErrorType Type { get; set; }

        /// <summary>
        /// Gets or sets the parameter name.
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// Gets or sets the information about the error.
        /// </summary>
        public string Info { get; set; }
    }
}
