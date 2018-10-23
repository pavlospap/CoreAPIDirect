namespace CoreApiDirect.Tests.Url
{
    internal static class DoubleEncoded
    {
        public const string NOT_EQUAL = "%253C%253E";
        public const string GREATER_OR_EQUAL = "%253E%253D";
        public const string LESS_OR_EQUAL = "%253C%253D";
        public const string EQUAL = "%253D";
        public const string GREATER = "%253E";
        public const string LESS = "%253C";
        public const string NOT_IN = "%2520not%2520in%2520";
        public const string IN = "%2520in%2520";
        public const string NOT_NULL = "%2520not%2520null%2520";
        public const string NULL = "%2520null%2520";
        public const string NOT_LIKE = "%2520not%2520like%2520";
        public const string LIKE = "%2520like%2520";
        public const string AND = "%2520and%2520";
        public const string OR = "%2520or%2520";
        public const string ASC = "%252B";
        public const string DESC = "%252D";
        public const string COMMA = "%252C";
    }
}
