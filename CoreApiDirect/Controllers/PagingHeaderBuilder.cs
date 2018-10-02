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

            valuesDict[nameof(queryString.PageNumber).Camelize()] = pageNumber;
            valuesDict[nameof(queryString.PageSize).Camelize()] = queryString.PageSize;
            valuesDict[nameof(queryString.QueryParams.Search).Camelize()] = queryString.QueryParams.Search;
            valuesDict[nameof(queryString.QueryParams.Fields).Camelize()] = string.Join(',', queryString.QueryParams.Fields);
            valuesDict[nameof(queryString.QueryParams.Sort).Camelize()] = string.Join(',', queryString.QueryParams.Sort.Select(p => p.Field + p.Direction.Encoded()));
            valuesDict[nameof(queryString.QueryParams.Filter).Camelize()] = _plainFilterBuilder.Build(queryString.QueryParams.Filter.ToArray());

            return controller.Url.Action("GetAsync", controller.ControllerContext.ActionDescriptor.ControllerName, values);
        }
    }
}
