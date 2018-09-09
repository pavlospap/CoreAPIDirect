namespace CoreApiDirect.Response
{
    /// <summary>
    /// Provides functionality to build the response for the current HTTP request.
    /// </summary>
    public interface IResponseBuilder
    {
        /// <summary>
        /// Adds an informational message with optional additional information to the response builder.
        /// </summary>
        /// <param name="message">The informational message.</param>
        /// <param name="additionalInfo">Optional additional information about the message.</param>
        /// <returns>The same response builder so that multiple calls can be chained.</returns>
        IResponseBuilder AddInfo(string message, params string[] additionalInfo);

        /// <summary>
        /// Adds a warning message with optional additional information to the response builder.
        /// </summary>
        /// <param name="message">The warning message.</param>
        /// <param name="additionalInfo">Optional additional information about the message.</param>
        /// <returns>The same response builder so that multiple calls can be chained.</returns>
        IResponseBuilder AddWarning(string message, params string[] additionalInfo);

        /// <summary>
        /// Adds an error message with optional additional information to the response builder.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="additionalInfo">Optional additional information about the message.</param>
        /// <returns>The same response builder so that multiple calls can be chained.</returns>
        IResponseBuilder AddError(string message, params string[] additionalInfo);

        /// <summary>
        /// Adds a message of the specified type with optional additional information to the response builder.
        /// </summary>
        /// <param name="type">The type of the message.</param>
        /// <param name="message">The message.</param>
        /// <param name="additionalInfo">Optional additional information about the message.</param>
        /// <returns>The same response builder so that multiple calls can be chained.</returns>
        IResponseBuilder AddMessage(MessageType type, string message, params string[] additionalInfo);

        /// <summary>
        /// Adds data to the response builder.
        /// </summary>
        /// <param name="data">The data object.</param>
        /// <returns>The same response builder so that multiple calls can be chained.</returns>
        IResponseBuilder AddData(object data);

        /// <summary>
        /// Gets a value that indicates whether the response contains error messages.
        /// </summary>
        /// <returns>True if the response contains error messages. Otherwise, false.</returns>
        bool HasErrors();

        /// <summary>
        /// Builds the response.
        /// </summary>
        /// <returns>The response object.</returns>
        object Build();
    }
}
