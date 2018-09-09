using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Primitives;

namespace CoreApiDirect.Url.Parsing.Parameters
{
    internal abstract class ParameterParser : IParameterParser
    {
        protected List<QueryStringError> Errors = new List<QueryStringError>();

        private string _parameterName;

        public ParameterParsingResult Parse(Type type, string parameterName, StringValues queryValues)
        {
            _parameterName = parameterName;

            var values = GetSeparatedValues(queryValues);
            RemoveEmptyValues(values);
            TrimValues(ref values);
            KeepDistinctValues(ref values);

            return new ParameterParsingResult
            {
                Value = ParseValues(type, values),
                Errors = Errors
            };
        }

        private List<string> GetSeparatedValues(StringValues queryValues)
        {
            var values = new List<string>();

            foreach (var value in queryValues)
            {
                values.AddRange(value.Split(','));
            }

            return values;
        }

        private void RemoveEmptyValues(List<string> values)
        {
            values.RemoveAll(p => string.IsNullOrWhiteSpace(p));
        }

        private void TrimValues(ref List<string> values)
        {
            values = values.Select(p => p.Trim()).ToList();
        }

        private void KeepDistinctValues(ref List<string> values)
        {
            values = values.Distinct().ToList();
        }

        protected abstract object ParseValues(Type type, List<string> values);

        protected void AddError(QueryStringErrorType errorType, string errorInfo)
        {
            Errors.Add(new QueryStringError
            {
                Type = errorType,
                ParameterName = _parameterName,
                Info = errorInfo
            });
        }
    }
}
