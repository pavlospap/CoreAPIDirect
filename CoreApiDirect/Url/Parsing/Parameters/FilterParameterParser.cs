using System;
using System.Collections.Generic;
using System.Linq;
using CoreApiDirect.Query.Operators;
using CoreApiDirect.Query.Parameters;
using CoreApiDirect.Url.Encoding;
using CoreApiDirect.Url.Parsing.Parameters.Validation;

namespace CoreApiDirect.Url.Parsing.Parameters
{
    internal class FilterParameterParser : ParameterParser, IFilterParameterParser
    {
        private readonly IFilterFieldValidator _fieldValidator;

        private readonly string[] _encodedLogicalOperators;
        private readonly string[] _encodedComparisonOperators;

        private int _lastEncodedLogicalOperatorIndex = 0;

        public FilterParameterParser(IFilterFieldValidator fieldValidator)
        {
            _fieldValidator = fieldValidator;
            _encodedLogicalOperators = GetEncodedOperators<LogicalOperator>();
            _encodedComparisonOperators = GetEncodedOperators<ComparisonOperator>();
        }

        private string[] GetEncodedOperators<T>()
            where T : IConvertible
        {
            return Enum.GetValues(typeof(T)).Cast<T>()
                .Select(p => p.Encoded()).ToArray();
        }

        protected override object ParseValues(Type type, List<string> values)
        {
            var filter = new List<QueryLogicalFilter>();
            values.ForEach(value => ParsePlainFilter(type, value, filter));

            return filter;
        }

        private void ParsePlainFilter(Type type, string plainFilter, List<QueryLogicalFilter> filter)
        {
            var logicalParts = plainFilter.Split(_encodedLogicalOperators, StringSplitOptions.None);
            _lastEncodedLogicalOperatorIndex = 0;

            for (int i = 0; i <= logicalParts.Length - 1; i++)
            {
                if (!ParseLogicalPart(type, filter, plainFilter, logicalParts[i], isFirstPart: i == 0))
                {
                    break;
                }
            }
        }

        private bool ParseLogicalPart(Type type, List<QueryLogicalFilter> filter, string plainFilter, string logicalPart, bool isFirstPart)
        {
            if (logicalPart.TotalOccurrencesOf(_encodedComparisonOperators) != 1)
            {
                AddError(QueryStringErrorType.InvalidFormat, plainFilter);
                return false;
            }

            string encodedComparisonOperator = logicalPart.FirstOccurrenceOf(_encodedComparisonOperators);
            var comparisonParts = logicalPart.Split(encodedComparisonOperator, StringSplitOptions.RemoveEmptyEntries);

            var comparisonOperator = GetOperator<ComparisonOperator>(encodedComparisonOperator);

            if (!ValidateFormat(plainFilter, comparisonOperator, encodedComparisonOperator, logicalPart, comparisonParts))
            {
                return false;
            }

            string comparisonValue = !IsNullOrNotNullCheck(comparisonOperator) ? comparisonParts[1].Trim() : null;

            string field = comparisonParts[0].Trim();

            if (!_fieldValidator.ValidateField(field, type))
            {
                AddError(QueryStringErrorType.InvalidField, field);
                return false;
            }

            var logicalOperator = isFirstPart ? LogicalOperator.And : GetLogicalOperator(plainFilter);

            filter.Add(new QueryLogicalFilter
            {
                Operator = logicalOperator,
                Filter = new QueryComparisonFilter
                {
                    Field = field,
                    Operator = comparisonOperator,
                    Values = GetValues(comparisonOperator, comparisonValue)
                }
            });

            return true;
        }

        private T GetOperator<T>(string encodedOperator)
            where T : IConvertible
        {
            return Enum.GetValues(typeof(T)).Cast<T>()
                .FirstOrDefault(p => p.Encoded() == encodedOperator);
        }

        private bool IsNullOrNotNullCheck(ComparisonOperator comparisonOperator)
        {
            return comparisonOperator == ComparisonOperator.Null || comparisonOperator == ComparisonOperator.NotNull;
        }

        private bool ValidateFormat(string plainFilter, ComparisonOperator comparisonOperator, string encodedComparisonOperator, string logicalPart, string[] comparisonParts)
        {
            if ((!IsNullOrNotNullCheck(comparisonOperator) && comparisonParts.Length != 2) ||
                (IsNullOrNotNullCheck(comparisonOperator) && (comparisonParts.Length != 1 || !logicalPart.EndsWith(encodedComparisonOperator))))
            {
                AddError(QueryStringErrorType.InvalidFormat, plainFilter);
                return false; ;
            }

            return true;
        }

        private IEnumerable<string> GetValues(ComparisonOperator comparisonOperator, string comparisonValue)
        {
            if (IsNullOrNotNullCheck(comparisonOperator))
            {
                return new string[] { };
            }

            return !IsInOrNotInCheck(comparisonOperator) ?
                new string[] { comparisonValue } :
                comparisonValue.Split(Encoded.COMMA);
        }

        private bool IsInOrNotInCheck(ComparisonOperator comparisonOperator)
        {
            return comparisonOperator == ComparisonOperator.In || comparisonOperator == ComparisonOperator.NotIn;
        }

        private LogicalOperator GetLogicalOperator(string value)
        {
            string and = _encodedLogicalOperators[0];
            string or = _encodedLogicalOperators[1];

            int andIndex = value.IndexOf(and, _lastEncodedLogicalOperatorIndex + 1);
            int orIndex = value.IndexOf(or, _lastEncodedLogicalOperatorIndex + 1);

            if (andIndex < 0)
            {
                _lastEncodedLogicalOperatorIndex = orIndex;
                return GetOperator<LogicalOperator>(or);
            }
            else if (orIndex < 0)
            {
                _lastEncodedLogicalOperatorIndex = andIndex;
                return GetOperator<LogicalOperator>(and);
            }
            else
            {
                _lastEncodedLogicalOperatorIndex = Math.Min(andIndex, orIndex);
                return GetOperator<LogicalOperator>(_lastEncodedLogicalOperatorIndex == andIndex ? and : or);
            }
        }
    }
}
