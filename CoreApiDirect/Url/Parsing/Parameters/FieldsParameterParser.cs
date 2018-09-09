using System;
using System.Collections.Generic;
using System.Linq;
using CoreApiDirect.Url.Parsing.Parameters.Validation;

namespace CoreApiDirect.Url.Parsing.Parameters
{
    internal class FieldsParameterParser : ParameterParser, IFieldsParameterParser
    {
        private readonly IFieldsFieldValidator _fieldValidator;

        public FieldsParameterParser(IFieldsFieldValidator fieldValidator)
        {
            _fieldValidator = fieldValidator;
        }

        protected override object ParseValues(Type type, List<string> queryValues)
        {
            var fields = new List<string>();

            foreach (string field in queryValues)
            {
                if (_fieldValidator.ValidateField(field, type))
                {
                    fields.Add(field);
                }
                else
                {
                    AddError(QueryStringErrorType.InvalidField, field);
                }
            }

            RemoveRedundantFields(fields);

            return fields;
        }

        private void RemoveRedundantFields(List<string> fields)
        {
            if (fields.Contains("*"))
            {
                fields.RemoveAll(p => p != "*");
                return;
            }

            var asteriskPrefixes = fields.Where(p => p.Contains("*")).Select(p => p.Replace("*", "")).ToList();
            asteriskPrefixes.Sort();

            foreach (var prefix in asteriskPrefixes)
            {
                fields.RemoveAll(p => !p.Equals(prefix + "*") && p.StartsWith(prefix));
            }
        }
    }
}
