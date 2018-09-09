using System;
using System.Linq;
using CoreApiDirect.Base;

namespace CoreApiDirect.Url.Parsing.Parameters.Validation
{
    internal class FilterFieldValidator : FieldValidator, IFilterFieldValidator
    {
        public FilterFieldValidator(IPropertyProvider propertyProvider)
            : base(propertyProvider)
        {
        }

        public override bool ValidateField(string field, Type type)
        {
            if (field.Count(p => p == '!') > 1)
            {
                return false;
            }

            return base.ValidateField(field, type);
        }

        protected override bool ValidateAsterisk(string fieldPart, bool isLast)
        {
            return !fieldPart.Contains('*');
        }

        protected override bool ValidateExclamation(string fieldPart, bool isLast)
        {
            return !(fieldPart.Contains('!') && (isLast || !fieldPart.EndsWith('!')));
        }

        protected override bool ValidateFieldPart(ref Type type, string fieldPart, bool isLast)
        {
            return base.ValidateFieldPart(ref type, fieldPart.Replace("!", ""), isLast);
        }
    }
}
