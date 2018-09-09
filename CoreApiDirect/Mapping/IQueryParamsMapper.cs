using System;
using CoreApiDirect.Query.Parameters;

namespace CoreApiDirect.Mapping
{
    /// <summary>
    /// Provides functionality to map the query parameters coming from the URL query string before they are used for querying the database.
    /// </summary>
    public interface IQueryParamsMapper
    {
        /// <summary>
        /// Performs the mapping of the query parameters.
        /// </summary>
        /// <param name="entityType">The entity type.</param>
        /// <param name="queryParams">A copy of the query parameters coming from the URL query string.</param>
        /// <returns>The processed query parameters.</returns>
        QueryParams MapQueryParams(Type entityType, QueryParams queryParams);
    }
}
