using System;
using CoreApiDirect.Mapping;
using CoreApiDirect.Query.Parameters;

namespace CoreApiDirect.Demo.Mapping
{
    internal class QueryParamsMapper : IQueryParamsMapper
    {
        public QueryParams MapQueryParams(Type entityType, QueryParams queryParams)
        {
            // Trasform QueryParams here...

            return queryParams;
        }
    }
}
