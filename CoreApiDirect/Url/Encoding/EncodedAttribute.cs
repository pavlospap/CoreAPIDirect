using System;

namespace CoreApiDirect.Url.Encoding
{
    internal class EncodedAttribute : Attribute
    {
        public string Encoded { get; set; }

        public EncodedAttribute(string encoded)
        {
            Encoded = encoded;
        }
    }
}
