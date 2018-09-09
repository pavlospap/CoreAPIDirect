using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreApiDirect.Base
{
    internal static class IDictionaryExtentions
    {
        public static T GetValueIgnoreCase<T>(this IDictionary<string, T> dictionary, string key)
        {
            var keyValuePair = dictionary.FirstOrDefault(p => string.Equals(p.Key, key, StringComparison.OrdinalIgnoreCase));

            if (keyValuePair.Equals(default(KeyValuePair<string, T>)))
            {
                throw new ArgumentException($"The dictionary does not contain key '{key}'.");
            }

            return keyValuePair.Value;
        }
    }
}
