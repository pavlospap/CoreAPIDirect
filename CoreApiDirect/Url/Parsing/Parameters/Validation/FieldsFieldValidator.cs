using System;
using CoreApiDirect.Base;

namespace CoreApiDirect.Url.Parsing.Parameters.Validation
{
    internal class FieldsFieldValidator : FieldValidator, IFieldsFieldValidator
    {
        public FieldsFieldValidator(IPropertyProvider propertyProvider)
            : base(propertyProvider)
        {
        }

        protected override bool ValidateAsterisk(string fieldPart, bool isLast)
        {
            return !(fieldPart == "*" && !isLast);
        }

        protected override bool ValidateExclamation(string fieldPart, bool isLast)
        {
            return !fieldPart.Contains('!');
        }

        protected override bool ValidateFieldPart(ref Type type, string fieldPart, bool isLast)
        {
            return fieldPart == "*" || base.ValidateFieldPart(ref type, fieldPart, isLast);
        }
    }
}
