using System;
using System.Linq;
using CoreApiDirect.Base;

namespace CoreApiDirect.Url.Parsing.Parameters.Validation
{
    internal abstract class FieldValidator : IFieldValidator
    {
        private readonly IPropertyProvider _propertyProvider;

        public FieldValidator(IPropertyProvider propertyProvider)
        {
            _propertyProvider = propertyProvider;
        }

        public virtual bool ValidateField(string field, Type type)
        {
            var fieldParts = field.Split('.').Select(p => p.Trim()).ToArray();

            for (int i = 0; i <= fieldParts.Length - 1; i++)
            {
                var fieldPart = fieldParts[i];
                bool isLast = i == fieldParts.Length - 1;

                if (!ValidateAsterisk(fieldPart, isLast) ||
                    !ValidateExclamation(fieldPart, isLast) ||
                    !ValidateFieldPart(ref type, fieldPart, isLast))
                {
                    return false;
                }
            }

            return true;
        }

        protected abstract bool ValidateAsterisk(string fieldPart, bool isLast);

        protected abstract bool ValidateExclamation(string fieldPart, bool isLast);

        protected virtual bool ValidateFieldPart(ref Type type, string fieldPart, bool isLast)
        {
            var property = _propertyProvider.GetProperties(type).FirstOrDefault(p => p.Name.Equals(fieldPart, StringComparison.OrdinalIgnoreCase));

            if (property == null)
            {
                return false;
            }

            var genericTypeDefinition = type.BaseGenericType().GetGenericTypeDefinition();
            bool isDetail = property.PropertyType.IsDetailOfRawGeneric(genericTypeDefinition);

            if (isDetail && !isLast)
            {
                type = property.PropertyType.IsListOfRawGeneric(genericTypeDefinition) ?
                    property.PropertyType.BaseGenericType().GenericTypeArguments[0] :
                    property.PropertyType;

                return true;
            }
            else if (!isDetail && isLast)
            {
                return true;
            }

            return false;
        }
    }
}
