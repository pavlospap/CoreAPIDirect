namespace CoreApiDirect.Url.Parsing.Parameters
{
    internal static class StringExtentions
    {
        public static int TotalOccurrencesOf(this string text, params string[] subTexts)
        {
            int totalCount = 0;

            for (int i = 0; i <= subTexts.Length - 1; i++)
            {
                int index = 0;
                while ((index = text.IndexOf(subTexts[i])) >= 0)
                {
                    totalCount++;
                    text = text.Remove(index, subTexts[i].Length);
                }
            }

            return totalCount;
        }

        public static string FirstOccurrenceOf(this string text, params string[] subTexts)
        {
            for (int i = 0; i <= subTexts.Length - 1; i++)
            {
                if (text.Contains(subTexts[i]))
                {
                    return subTexts[i];
                }
            }

            return null;
        }
    }
}
