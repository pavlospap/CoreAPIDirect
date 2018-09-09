using System;

namespace CoreApiDirect.Url.Encoding
{
    internal static class EnumExtensions
    {
        public static string Encoded<TEnum>(this TEnum value)
            where TEnum : IConvertible
        {
            var type = value.GetType();

            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException($"'{type.Name}' is not an enumerated type.");
            }

            string name = Enum.GetName(type, value);
            var field = type.GetField(name);

            if (Attribute.GetCustomAttribute(field, typeof(EncodedAttribute)) is EncodedAttribute attribute)
            {
                return attribute.Encoded;
            }

            throw new ArgumentException($"Enum value '{value.ToString()}' does not have an attribute '{nameof(EncodedAttribute)}'.");
        }
    }
}
