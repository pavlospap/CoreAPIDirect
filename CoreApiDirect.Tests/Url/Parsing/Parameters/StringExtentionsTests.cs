using CoreApiDirect.Url.Parsing.Parameters;
using Xunit;

namespace CoreApiDirect.Tests.Url.Parsing.Parameters
{
    public class StringExtentionsTests
    {
        [Fact]
        public void TotalOccurrencesOf_NoParams_Zero()
        {
            Assert.Equal(0, "12345".TotalOccurrencesOf());
        }

        [Fact]
        public void TotalOccurrencesOf_SubTextsNotExist_Zero()
        {
            Assert.Equal(0, "12345".TotalOccurrencesOf("6", "7"));
        }

        [Fact]
        public void TotalOccurrencesOf_SubTextsExist_NumberOfOccurrences()
        {
            Assert.Equal(6, "1234512345".TotalOccurrencesOf("1", "3", "5"));
        }

        [Fact]
        public void FirstOccurrenceOf_NoParams_Null()
        {
            Assert.Null("12345".FirstOccurrenceOf());
        }

        [Fact]
        public void FirstOccurrenceOf_SubTextsNotExist_Null()
        {
            Assert.Null("12345".FirstOccurrenceOf("6", "7"));
        }

        [Fact]
        public void FirstOccurrenceOf_SubTextsExist_FirstOccurrence()
        {
            Assert.Equal("3", "12345".FirstOccurrenceOf("3", "5"));
        }
    }
}
