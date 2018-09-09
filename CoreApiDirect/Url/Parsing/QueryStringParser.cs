using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CoreApiDirect.Base;
using CoreApiDirect.Options;
using CoreApiDirect.Query.Parameters;
using CoreApiDirect.Url.Parsing.Parameters;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace CoreApiDirect.Url.Parsing
{
    internal class QueryStringParser : IQueryStringParser
    {
        private readonly QueryString _queryString;
        private readonly IPropertyProvider _propertyProvider;
        private readonly IParameterParserFactory _parameterParserFactory;
        private readonly string[] _knownParameters;

        public QueryStringParser(
            IOptions<CoreOptions> options,
            IPropertyProvider propertyProvider,
            IParameterParserFactory parameterParserFactory)
        {
            _queryString = new QueryString(options.Value);
            _propertyProvider = propertyProvider;
            _parameterParserFactory = parameterParserFactory;
            _knownParameters = Enum.GetNames(typeof(QueryStringKnownParameter));
        }

        public QueryString Parse(Type type, IEnumerable<KeyValuePair<string, StringValues>> parameters)
        {
            foreach (var parameter in parameters)
            {
                ParseParameter(parameter, type);
            }

            return _queryString;
        }

        private void ParseParameter(KeyValuePair<string, StringValues> parameter, Type type)
        {
            string parameterName = parameter.Key.Trim();

            if (_knownParameters.Contains(parameterName, StringComparer.OrdinalIgnoreCase))
            {
                return;
            }

            PropertyInfo property = null;

            if (ParameterIsQueryStringParameter(parameterName))
            {
                property = _propertyProvider.GetProperties(typeof(QueryString)).FirstOrDefault(p => p.Name.Equals(parameterName, StringComparison.OrdinalIgnoreCase));
                ParseValue(type, property, parameterName, parameter.Value, _queryString);
                return;
            }
            else if (ParameterIsQueryParamsParameter(parameterName))
            {
                property = _propertyProvider.GetProperties(typeof(QueryParams)).FirstOrDefault(p => p.Name.Equals(parameterName, StringComparison.OrdinalIgnoreCase));
                ParseValue(type, property, parameterName, parameter.Value, _queryString.QueryParams);
                return;
            }

            AddInvalidParameterError(parameterName);
        }

        private bool ParameterIsQueryStringParameter(string parameterName)
        {
            return _propertyProvider.GetProperties(typeof(QueryString))
                .Where(p => p.IsDefined(typeof(QueryStringParameterAttribute), false))
                .Select(p => p.Name)
                .Contains(parameterName, StringComparer.OrdinalIgnoreCase);
        }

        private void ParseValue(Type type, PropertyInfo property, string parameterName, StringValues parameterValues, object obj)
        {
            var result = _parameterParserFactory.Create(property.Name).Parse(type, parameterName, parameterValues);

            if (result.Value != null)
            {
                obj.SetPropertyValue(property.Name, result.Value);
            }

            _queryString.Errors.AddRange(result.Errors);
        }

        private bool ParameterIsQueryParamsParameter(string parameterName)
        {
            return _propertyProvider.GetProperties(typeof(QueryParams))
                .Select(p => p.Name)
                .Contains(parameterName, StringComparer.OrdinalIgnoreCase);
        }

        private void AddInvalidParameterError(string parameterName)
        {
            _queryString.Errors.Add(new QueryStringError
            {
                Type = QueryStringErrorType.InvalidParameter,
                Info = parameterName
            });
        }
    }
}
