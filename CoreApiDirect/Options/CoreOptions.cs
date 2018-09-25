using System;
using CoreApiDirect.Response;

namespace CoreApiDirect.Options
{
    /// <summary>
    /// The CoreAPIDirect options.
    /// </summary>
    public class CoreOptions
    {
        internal const string DEFAULT_ROUTE_PREFIX = "api";
        internal const bool DEFAULT_VALIDATE_ROUTE = false;
        internal const bool DEFAULT_VALIDATE_QUERYSTRING = false;
        internal const int DEFAULT_RELATED_DATA_LEVEL = 0;
        internal const int DEFAULT_MAX_PAGE_SIZE = 20;
        internal const int DEFAULT_PAGE_SIZE = 10;
        internal const bool DEFAULT_LOG_DATA = true;
        internal const bool DEFAULT_CASE_SENSITIVE_SEARCH = false;
        internal const int MIN_RELATED_DATA_LEVEL = 0;
        internal const int MIN_PAGE_SIZE = 1;

        /// <summary>
        /// Gets or sets the prefix for every API route.
        /// </summary>
        public string RoutePrefix { get; set; } = DEFAULT_ROUTE_PREFIX;

        /// <summary>
        /// Gets or sets a value that indicates whether the route params are going to be validated.
        /// </summary>
        public bool ValidateRoute { get; set; } = DEFAULT_VALIDATE_ROUTE;

        /// <summary>
        /// Gets or sets a value that indicates whether the URL query string is going to be validated.
        /// </summary>
        public bool ValidateQueryString { get; set; } = DEFAULT_VALIDATE_QUERYSTRING;

        /// <summary>
        /// Gets or sets a value that indicates how many levels of related data are going to be included in the response.
        /// </summary>
        public int RelatedDataLevel
        {
            get
            {
                return _relatedDataLevel;
            }
            set
            {
                _relatedDataLevel = value < MIN_RELATED_DATA_LEVEL ? MIN_RELATED_DATA_LEVEL : value;
            }
        }

        /// <summary>
        /// Gets or sets the max page size for an HTTP GET request.
        /// </summary>
        public int MaxPageSize
        {
            get
            {
                return _maxPageSize;
            }
            set
            {
                _maxPageSize = value < MIN_PAGE_SIZE ? MIN_PAGE_SIZE : value;
            }
        }

        /// <summary>
        /// Gets or sets the page size for an HTTP GET request.
        /// </summary>
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {

                _pageSize = value > _maxPageSize ? _maxPageSize : (value < MIN_PAGE_SIZE ? MIN_PAGE_SIZE : value);
            }
        }

        /// <summary>
        /// Gets or sets the implementation of the CoreApiDirect.Response.IResponseBuilder that will be used.
        /// </summary>
        public Type ResponseBuilderType { get; set; } = typeof(ResponseBuilder);

        /// <summary>
        /// Gets or sets a value that indicates whether the library will log, at debug level, the models sent and the data returned from the HTTP methods.
        /// </summary>
        public bool LogData { get; set; } = DEFAULT_LOG_DATA;

        /// <summary>
        /// Gets or sets a value that indicates whether the search, that is performed through a URL query string parameter, is case sensitive.
        /// </summary>
        public bool CaseSensitiveSearch { get; set; } = DEFAULT_CASE_SENSITIVE_SEARCH;

        private int _relatedDataLevel;
        private int _maxPageSize;
        private int _pageSize;

        /// <summary>
        /// Initializes a new instance of the CoreApiDirect.Options.CoreOptions class.
        /// </summary>
        public CoreOptions()
        {
            _relatedDataLevel = DEFAULT_RELATED_DATA_LEVEL;
            _maxPageSize = DEFAULT_MAX_PAGE_SIZE;
            _pageSize = DEFAULT_PAGE_SIZE;
        }
    }
}
