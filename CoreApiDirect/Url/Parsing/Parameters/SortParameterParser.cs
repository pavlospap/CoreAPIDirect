using System;
using System.Collections.Generic;
using CoreApiDirect.Query.Operators;
using CoreApiDirect.Query.Parameters;
using CoreApiDirect.Url.Encoding;
using CoreApiDirect.Url.Parsing.Parameters.Validation;

namespace CoreApiDirect.Url.Parsing.Parameters
{
    internal class SortParameterParser : ParameterParser, ISortParameterParser
    {
        private readonly ISortFieldValidator _fieldValidator;

        public SortParameterParser(ISortFieldValidator fieldValidator)
        {
            _fieldValidator = fieldValidator;
        }

        protected override object ParseValues(Type type, List<string> values)
        {
            var sort = new List<QuerySort>();

            string asc = SortDirection.Ascending.Encoded();
            string desc = SortDirection.Descending.Encoded();

            values.ForEach(value => ParsePlainSort(type, value, sort, asc, desc));

            return sort;
        }

        private void ParsePlainSort(Type type, string plainSort, List<QuerySort> sort, string asc, string desc)
        {
            if (plainSort.TotalOccurrencesOf(new string[] { asc, desc }) > 1 ||
                (!plainSort.EndsWith(asc) && !plainSort.EndsWith(desc)))
            {
                AddError(QueryStringErrorType.InvalidFormat, plainSort);
            }
            else
            {
                var direction = plainSort.IndexOf(asc) > 0 ? SortDirection.Ascending : SortDirection.Descending;
                string field = plainSort.Replace(direction.Encoded(), "").Trim();

                if (!_fieldValidator.ValidateField(field, type))
                {
                    AddError(QueryStringErrorType.InvalidField, field);
                    return;
                }

                sort.Add(new QuerySort
                {
                    Direction = direction,
                    Field = field
                });
            }
        }
    }
}
