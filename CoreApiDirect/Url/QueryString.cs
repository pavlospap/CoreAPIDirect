using System.Collections.Generic;
using CoreApiDirect.Options;
using CoreApiDirect.Query.Parameters;
using Microsoft.AspNetCore.Mvc;

namespace CoreApiDirect.Url
{
    /// <summary>
    /// Represents the URL query string.
    /// </summary>
    [ModelBinder(BinderType = typeof(QueryStringModelBinder))]
    public class QueryString
    {
        internal const int MIN_PAGE_NUMBER = 1;

        /// <summary>
        /// Gets or sets the query parameters.
        /// </summary>
        public QueryParams QueryParams { get; set; } = new QueryParams();

        /// <summary>
        /// Gets or sets a value that indicates whether the route params are going to be validated.
        /// </summary>
        [QueryStringParameter]
        public bool ValidateRoute { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether the URL query string is going to be validated.
        /// </summary>
        [QueryStringParameter]
        public bool ValidateQueryString { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates how many levels of related data are going to be included in the response.
        /// </summary>
        [QueryStringParameter]
        public int RelatedDataLevel
        {
            get
            {
                return _relatedDataLevel;
            }
            set
            {
                _relatedDataLevel = value < CoreOptions.MIN_RELATED_DATA_LEVEL ? CoreOptions.MIN_RELATED_DATA_LEVEL : value;
            }
        }

        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        [QueryStringParameter]
        public int PageNumber
        {
            get
            {
                return _pageNumber;
            }
            set
            {
                _pageNumber = value < MIN_PAGE_NUMBER ? MIN_PAGE_NUMBER : value;
            }
        }

        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        [QueryStringParameter]
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {

                _pageSize = value > _options.MaxPageSize ? _options.MaxPageSize : (value < CoreOptions.MIN_PAGE_SIZE ? CoreOptions.MIN_PAGE_SIZE : value);
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the search is case sensitive.
        /// </summary>
        [QueryStringParameter]
        public bool CaseSensitiveSearch { get; set; }

        /// <summary>
        /// Gets or sets the query string errors.
        /// </summary>
        public List<QueryStringError> Errors { get; set; } = new List<QueryStringError>();

        private readonly CoreOptions _options;

        private int _relatedDataLevel;
        private int _pageNumber = 1;
        private int _pageSize;

        /// <summary>
        /// Initializes a new instance of the CoreApiDirect.Url.QueryString class.
        /// </summary>
        /// <param name="options">The CoreApiDirect.Options.CoreOptions.</param>
        public QueryString(CoreOptions options)
        {
            _options = options;
            ValidateRoute = options.ValidateRoute;
            ValidateQueryString = options.ValidateQueryString;
            CaseSensitiveSearch = options.CaseSensitiveSearch;
            _relatedDataLevel = options.RelatedDataLevel;
            _pageSize = options.PageSize;
        }
    }
}
