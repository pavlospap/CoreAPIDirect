using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using CoreApiDirect.Base;
using CoreApiDirect.Url;
using CoreApiDirect.Url.Encoding;
using Humanizer;
using Microsoft.AspNetCore.Mvc;

namespace CoreApiDirect.Controllers
{
    internal class PagingHeaderBuilder : IPagingHeaderBuilder
    {
        private readonly IPlainFilterBuilder _plainFilterBuilder;

        public PagingHeaderBuilder(IPlainFilterBuilder plainFilterBuilder)
        {
            _plainFilterBuilder = plainFilterBuilder;
        }

        public string Build<TEntity>(ControllerBase controller, QueryString queryString, PagedList<TEntity> entityList)
        {
            string baseUrl = $"{controller.Request.Scheme}://{controller.Request.Host}";
            string previousPageUrl = entityList.HasPrevious ? baseUrl + BuildUrl(controller, queryString, queryString.PageNumber - 1) : null;
            string nextPageUrl = entityList.HasNext ? baseUrl + BuildUrl(controller, queryString, queryString.PageNumber + 1) : null;

            var pagingMetadata = new
            {
                totalCount = entityList.TotalCount,
                pageSize = entityList.PageSize,
                currentPage = entityList.PageNumber,
                totalPages = entityList.TotalPages,
                previousPageUrl,
                nextPageUrl
            };

            return pagingMetadata.ToJson();
        }

        private string BuildUrl(ControllerBase controller, QueryString queryString, int pageNumber)
        {
            var values = new ExpandoObject();
            var valuesDict = (IDictionary<string, object>)values;

            valuesDict[nameof(QueryString.ValidateRoute).Camelize()] = queryString.ValidateRoute;
            valuesDict[nameof(QueryString.ValidateQueryString).Camelize()] = queryString.ValidateQueryString;
            valuesDict[nameof(QueryString.RelatedDataLevel).Camelize()] = queryString.RelatedDataLevel;
            valuesDict[nameof(QueryString.PageNumber).Camelize()] = pageNumber;
            valuesDict[nameof(QueryString.PageSize).Camelize()] = queryString.PageSize;
            valuesDict[nameof(QueryString.CaseSensitiveSearch).Camelize()] = queryString.CaseSensitiveSearch;
            valuesDict[nameof(QueryString.QueryParams.Search).Camelize()] = queryString.QueryParams.Search;
            valuesDict[nameof(QueryString.QueryParams.Fields).Camelize()] = string.Join(',', queryString.QueryParams.Fields);
            valuesDict[nameof(QueryString.QueryParams.Sort).Camelize()] = string.Join(',', queryString.QueryParams.Sort.Select(p => p.Field + p.Direction.Encoded()));
            valuesDict[nameof(QueryString.QueryParams.Filter).Camelize()] = _plainFilterBuilder.Build(queryString.QueryParams.Filter.ToArray());

            return controller.Url.Action("GetAsync", controller.ControllerContext.ActionDescriptor.ControllerName, values);
        }
    }
}
