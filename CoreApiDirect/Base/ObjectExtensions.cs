using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CoreApiDirect.Base
{
    /// <summary>
    /// Extensions for the System.Object.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Throws an exception of type ArgumentNullException if the argument is null.
        /// </summary>
        /// <param name="argument">The specified argument.</param>
        /// <param name="argumentName">The argument name.</param>
        public static void ValidateNull(this object argument, string argumentName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        /// <summary>
        /// Gets the value of an object's property.
        /// </summary>
        /// <param name="obj">The specified object.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>The property value.</returns>
        public static object GetPropertyValue(this object obj, string propertyName)
        {
            return GetProperty(obj, propertyName).GetValue(obj, null);
        }

        /// <summary>
        /// Sets the value of an object's property.
        /// </summary>
        /// <param name="obj">The specified object.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="value">The property value.</param>
        public static void SetPropertyValue(this object obj, string propertyName, object value)
        {
            GetProperty(obj, propertyName).SetValue(obj, value);
        }

        private static PropertyInfo GetProperty(object obj, string propertyName)
        {
            propertyName.ValidateNull(nameof(propertyName));
            var property = obj.GetType().GetProperty(propertyName);

            return property ?? throw new ArgumentException($"'{obj.GetType().Name}' does not contain property '{propertyName}'.");
        }

        /// <summary>
        /// Gets the JSON representation of an object.
        /// </summary>
        /// <param name="obj">The specified object.</param>
        /// <param name="formatting">The Newtonsoft.Json.Formatting options.</param>
        /// <param name="contractResolver">An optional Newtonsoft.Json.Serialization.IContractResolver.
        /// If not present, the Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver is used.</param>
        /// <returns>The JSON representation of the object.</returns>
        public static string ToJson(this object obj, Formatting formatting = Formatting.None, IContractResolver contractResolver = null)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings()
            {
                Formatting = formatting,
                ContractResolver = contractResolver ?? new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        /// <summary>
        /// Gets the copy of an object.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="obj">The specified object.</param>
        /// <returns>A new copy of the specified object.</returns>
        public static T Copy<T>(this T obj)
        {
            return JsonConvert.DeserializeObject<T>(obj.ToJson());
        }
    }
}
