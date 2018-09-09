using CoreApiDirect.Base;

namespace CoreApiDirect.Url.Parsing.Parameters.Validation
{
    internal class SortFieldValidator : FieldValidator, ISortFieldValidator
    {
        public SortFieldValidator(IPropertyProvider propertyProvider)
            : base(propertyProvider)
        {
        }

        protected override bool ValidateAsterisk(string fieldPart, bool isLast)
        {
            return !fieldPart.Contains('*');
        }

        protected override bool ValidateExclamation(string fieldPart, bool isLast)
        {
            return !fieldPart.Contains('!');
        }
    }
}
