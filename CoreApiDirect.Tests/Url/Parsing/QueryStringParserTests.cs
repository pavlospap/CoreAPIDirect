using System;
using System.Collections.Generic;
using System.Linq;
using CoreApiDirect.Base;
using CoreApiDirect.Demo.Dto.Out.App;
using CoreApiDirect.Options;
using CoreApiDirect.Query.Operators;
using CoreApiDirect.Query.Parameters;
using CoreApiDirect.Tests.Options;
using CoreApiDirect.Url;
using CoreApiDirect.Url.Parsing;
using CoreApiDirect.Url.Parsing.Parameters;
using CoreApiDirect.Url.Parsing.Parameters.Validation;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace CoreApiDirect.Tests.Url.Parsing
{
    public class QueryStringParserTests
    {
        private IQueryStringParser _queryStringParser;

        public QueryStringParserTests()
        {
            _queryStringParser = new QueryStringParser(new CoreOptionsTests(), new PropertyProvider(),
                new ParameterParserFactory(
                    new BooleanParameterParser(),
                    new IntegerParameterParser(),
                    new StringParameterParser(),
                    new FieldsParameterParser(new FieldsFieldValidator(new PropertyProvider())),
                    new SortParameterParser(new SortFieldValidator(new PropertyProvider())),
                    new FilterParameterParser(new FilterFieldValidator(new PropertyProvider()))));
        }

        [Fact]
        public void Parse_InvalidParameter_Error()
        {
            Error("http://api?invalid=", QueryStringErrorType.InvalidParameter);
        }

        [Fact]
        public void Parse_KnownParameter_NoError()
        {
            var queryString = _queryStringParser.Parse(null, GetQuery("http://api?culture="));
            Assert.False(queryString.Errors.Any());
        }

        [Fact]
        public void Parse_MissingParameter_Error()
        {
            Error("http://api?=", QueryStringErrorType.InvalidParameter);
        }

        [Fact]
        public void Parse_RemoveEmptyFields_NoFields()
        {
            var queryString = _queryStringParser.Parse(typeof(SchoolOutDto), GetQuery("http://api?fields=&fields=,"));
            Assert.False(queryString.QueryParams.Fields.Any());
        }

        [Fact]
        public void Parse_MergeFields_EqualToExpected()
        {
            var queryString = _queryStringParser.Parse(typeof(SchoolOutDto), GetQuery("http://api?fields=name,yearofestablishment&fields=students.*"));
            Assert.Equal("name,yearofestablishment,students.*", string.Join(',', queryString.QueryParams.Fields));
        }

        [Fact]
        public void Parse_TrimFields_EqualToExpected()
        {
            var queryString = _queryStringParser.Parse(typeof(SchoolOutDto), GetQuery("http://api?fields= name , yearofestablishment &fields= students.* "));
            Assert.Equal("name,yearofestablishment,students.*", string.Join(',', queryString.QueryParams.Fields));
        }

        [Theory]
        [InlineData("http://api", CoreOptions.DEFAULT_VALIDATE_ROUTE)]
        [InlineData("http://api?validateRoute=", CoreOptions.DEFAULT_VALIDATE_ROUTE)]
        [InlineData("http://api?validateRoute=true", true)]
        [InlineData("http://api?validateRoute=true,false", true)]
        [InlineData("http://api?validateRoute=true,validateRoute=false", true)]
        [InlineData("http://api?validateRoute=invalid", CoreOptions.DEFAULT_VALIDATE_ROUTE)]
        public void Parse_ValidateRoute_EqualToExpected(string url, bool expected)
        {
            var queryString = _queryStringParser.Parse(null, GetQuery(url));
            Assert.Equal(expected, queryString.ValidateRoute);
        }

        [Theory]
        [InlineData("http://api", CoreOptions.DEFAULT_VALIDATE_QUERYSTRING)]
        [InlineData("http://api?validateQueryString=", CoreOptions.DEFAULT_VALIDATE_QUERYSTRING)]
        [InlineData("http://api?validateQueryString=true", true)]
        [InlineData("http://api?validateQueryString=true,false", true)]
        [InlineData("http://api?validateQueryString=true,validateQueryString=false", true)]
        [InlineData("http://api?validateQueryString=invalid", CoreOptions.DEFAULT_VALIDATE_QUERYSTRING)]
        public void Parse_ValidateQueryString_EqualToExpected(string url, bool expected)
        {
            var queryString = _queryStringParser.Parse(null, GetQuery(url));
            Assert.Equal(expected, queryString.ValidateQueryString);
        }

        [Theory]
        [InlineData("http://api", CoreOptions.DEFAULT_RELATED_DATA_LEVEL)]
        [InlineData("http://api?relatedDataLevel=", CoreOptions.DEFAULT_RELATED_DATA_LEVEL)]
        [InlineData("http://api?relatedDataLevel=5", 5)]
        [InlineData("http://api?relatedDataLevel=5,6", 5)]
        [InlineData("http://api?relatedDataLevel=5,relatedDataLevel=6", 5)]
        [InlineData("http://api?relatedDataLevel=-1", CoreOptions.MIN_RELATED_DATA_LEVEL)]
        [InlineData("http://api?relatedDataLevel=invalid", CoreOptions.DEFAULT_RELATED_DATA_LEVEL)]
        public void Parse_RelatedDataLevel_EqualToExpected(string url, int expected)
        {
            var queryString = _queryStringParser.Parse(null, GetQuery(url));
            Assert.Equal(expected, queryString.RelatedDataLevel);
        }

        [Theory]
        [InlineData("http://api", QueryString.MIN_PAGE_NUMBER)]
        [InlineData("http://api?pageNumber=", QueryString.MIN_PAGE_NUMBER)]
        [InlineData("http://api?pageNumber=5", 5)]
        [InlineData("http://api?pageNumber=5,10", 5)]
        [InlineData("http://api?pageNumber=5,pageNumber=10", 5)]
        [InlineData("http://api?pageNumber=0", QueryString.MIN_PAGE_NUMBER)]
        [InlineData("http://api?pageNumber=-1", QueryString.MIN_PAGE_NUMBER)]
        [InlineData("http://api?pageNumber=invalid", QueryString.MIN_PAGE_NUMBER)]
        public void Parse_PageNumber_EqualToExpected(string url, int expected)
        {
            var queryString = _queryStringParser.Parse(null, GetQuery(url));
            Assert.Equal(expected, queryString.PageNumber);
        }

        [Theory]
        [InlineData("http://api", CoreOptions.DEFAULT_PAGE_SIZE)]
        [InlineData("http://api?pageSize=", CoreOptions.DEFAULT_PAGE_SIZE)]
        [InlineData("http://api?pageSize=15", 15)]
        [InlineData("http://api?pageSize=15,16", 15)]
        [InlineData("http://api?pageSize=15,pageSize=16", 15)]
        [InlineData("http://api?pageSize=25", CoreOptions.DEFAULT_MAX_PAGE_SIZE)]
        [InlineData("http://api?pageSize=0", CoreOptions.MIN_PAGE_SIZE)]
        [InlineData("http://api?pageSize=-1", CoreOptions.MIN_PAGE_SIZE)]
        [InlineData("http://api?pageSize=invalid", CoreOptions.DEFAULT_PAGE_SIZE)]
        public void Parse_PageSize_EqualToExpected(string url, int expected)
        {
            var queryString = _queryStringParser.Parse(null, GetQuery(url));
            Assert.Equal(expected, queryString.PageSize);
        }

        [Theory]
        [InlineData("http://api?search=")]
        [InlineData("http://api?search= ")]
        [InlineData("http://api?search=keyword", "keyword")]
        [InlineData("http://api?search=keyword,ignored", "keyword")]
        public void Parse_Search_EqualToExpected(string url, string expected = null)
        {
            var queryString = _queryStringParser.Parse(null, GetQuery(url));
            Assert.Equal(expected, queryString.QueryParams.Search);
        }

        [Fact]
        public void Parse_RemoveRedundantFields_EqualToExpected()
        {
            var fields = "name,name,lessons.name,lessons.*,lessons.books.*,students.firstname,students.firstname,students.lastname,students.contactinfo.*,students.contactinfo.phones*";
            var expected = "name,lessons.*,students.firstname,students.lastname,students.contactinfo.*";
            var queryString = _queryStringParser.Parse(typeof(SchoolOutDto), GetQuery("http://api?fields=" + fields));
            Assert.Equal(expected, string.Join(',', queryString.QueryParams.Fields));
        }

        [Fact]
        public void Parse_DetailWithoutSuffix_Error()
        {
            Error("http://api?fields=students", QueryStringErrorType.InvalidField);
        }

        [Fact]
        public void Parse_AsteriskNotAtTheEnd_Error()
        {
            Error("http://api?fields=students.*.anyField", QueryStringErrorType.InvalidField);
        }

        [Fact]
        public void Parse_AsteriskMoreThanOne_Error()
        {
            Error("http://api?fields=students.*.*", QueryStringErrorType.InvalidField);
        }

        [Fact]
        public void Parse_AsteriskAfterNonDetail_Error()
        {
            Error("http://api?fields=name.*", QueryStringErrorType.InvalidField);
        }

        [Theory]
        [InlineData("http://api?fields=invalidField")]
        [InlineData("http://api?fields=students.invalidField")]
        [InlineData("http://api?fields=students.contactinfo.invalidField")]
        public void Parse_InvalidFields_Error(string url)
        {
            Error(url, QueryStringErrorType.InvalidField);
        }

        [Theory]
        [InlineData("http://api?fields=name.anyField")]
        [InlineData("http://api?fields=students.firstname.anyField")]
        [InlineData("http://api?fields=students.contactinfo.email.anyField")]
        public void Parse_NonDetailFieldsΙnTheMiddle_Error(string url)
        {
            Error(url, QueryStringErrorType.InvalidField);
        }

        [Theory]
        [InlineData("http://api?sort=name" + DoubleEncoded.ASC, "name", SortDirection.Ascending)]
        [InlineData("http://api?sort=students.firstname" + DoubleEncoded.DESC, "students.firstname", SortDirection.Descending)]
        [InlineData("http://api?sort=students.contactinfo.email" + DoubleEncoded.ASC, "students.contactinfo.email", SortDirection.Ascending)]
        [InlineData("http://api?sort=students.contactinfo.phones.number" + DoubleEncoded.ASC, "students.contactinfo.phones.number", SortDirection.Ascending)]
        public void Parse_ValidSort_EqualToExpected(string url, string field, SortDirection direction)
        {
            var queryString = _queryStringParser.Parse(typeof(SchoolOutDto), GetQuery(url));
            var sort = queryString.QueryParams.Sort.First();
            Assert.True(sort.Direction == direction && sort.Field == field);
        }

        [Fact]
        public void Parse_TrimSort_EqualToExpected()
        {
            var queryString = _queryStringParser.Parse(typeof(SchoolOutDto), GetQuery("http://api?sort= name " + DoubleEncoded.ASC));
            var sort = queryString.QueryParams.Sort.First();
            Assert.True(sort.Direction == SortDirection.Ascending && sort.Field == "name");
        }

        [Fact]
        public void Parse_SortWithAsterisk_Error()
        {
            Error("http://api?sort=students.*" + DoubleEncoded.ASC, QueryStringErrorType.InvalidField);
        }

        [Fact]
        public void Parse_SortWithNoOperator_Error()
        {
            Error("http://api?sort=name", QueryStringErrorType.InvalidFormat);
        }

        [Theory]
        [InlineData("http://api?sort=" + DoubleEncoded.ASC + "name")]
        [InlineData("http://api?sort=" + DoubleEncoded.DESC + "name")]
        public void Parse_SortWithNoOperatorAtEnd_Error(string url)
        {
            Error(url, QueryStringErrorType.InvalidFormat);
        }

        [Theory]
        [InlineData("http://api?sort=name" + DoubleEncoded.ASC + DoubleEncoded.ASC)]
        [InlineData("http://api?sort=name" + DoubleEncoded.DESC + DoubleEncoded.DESC)]
        [InlineData("http://api?sort=name" + DoubleEncoded.ASC + DoubleEncoded.DESC)]
        [InlineData("http://api?sort=name" + DoubleEncoded.DESC + DoubleEncoded.ASC)]
        public void Parse_SortWithMoreThanOneOperator_Error(string url)
        {
            Error(url, QueryStringErrorType.InvalidFormat);
        }

        [Theory]
        [InlineData("http://api?sort=invalidField" + DoubleEncoded.ASC)]
        [InlineData("http://api?sort=students.invalidField" + DoubleEncoded.ASC)]
        [InlineData("http://api?sort=students.contactinfo.invalidField" + DoubleEncoded.ASC)]
        [InlineData("http://api?sort=students.contactinfo.phones.invalidField" + DoubleEncoded.ASC)]
        public void Parse_SortWithInvalidFields_Error(string url)
        {
            Error(url, QueryStringErrorType.InvalidField);
        }

        [Theory]
        [InlineData("http://api?sort=students" + DoubleEncoded.ASC)]
        [InlineData("http://api?sort=students.contactinfo" + DoubleEncoded.ASC)]
        [InlineData("http://api?sort=students.contactinfo.phones" + DoubleEncoded.ASC)]
        public void Parse_SortWithDetailFields_Error(string url)
        {
            Error(url, QueryStringErrorType.InvalidField);
        }

        [Theory]
        [InlineData("http://api?sort=name.anyField" + DoubleEncoded.ASC)]
        [InlineData("http://api?sort=students.firstname.anyField" + DoubleEncoded.ASC)]
        [InlineData("http://api?sort=students.contactinfo.email.anyField" + DoubleEncoded.ASC)]
        [InlineData("http://api?sort=students.contactinfo.phones.number.anyField" + DoubleEncoded.ASC)]
        public void Parse_SortWithNonDetailFieldsΙnTheMiddle_Error(string url)
        {
            Error(url, QueryStringErrorType.InvalidField);
        }

        [Theory]
        [InlineData("http://api?filter=name" + DoubleEncoded.EQUAL + "University of Pennsylvania", LogicalOperator.And, "name", ComparisonOperator.Equal, "University of Pennsylvania")]
        [InlineData("http://api?filter=name" + DoubleEncoded.NOT_EQUAL + "University of Pennsylvania", LogicalOperator.And, "name", ComparisonOperator.NotEqual, "University of Pennsylvania")]
        [InlineData("http://api?filter=yearofestablishment" + DoubleEncoded.GREATER + "1891", LogicalOperator.And, "yearofestablishment", ComparisonOperator.Greater, "1891")]
        [InlineData("http://api?filter=yearofestablishment" + DoubleEncoded.GREATER_OR_EQUAL + "1891", LogicalOperator.And, "yearofestablishment", ComparisonOperator.GreaterOrEqual, "1891")]
        [InlineData("http://api?filter=yearofestablishment" + DoubleEncoded.LESS + "1891", LogicalOperator.And, "yearofestablishment", ComparisonOperator.Less, "1891")]
        [InlineData("http://api?filter=yearofestablishment" + DoubleEncoded.LESS_OR_EQUAL + "1891", LogicalOperator.And, "yearofestablishment", ComparisonOperator.LessOrEqual, "1891")]
        [InlineData("http://api?filter=name" + DoubleEncoded.IN + "University of Pennsylvania" + DoubleEncoded.COMMA + "University of Minnesota", LogicalOperator.And, "name", ComparisonOperator.In, "University of Pennsylvania,University of Minnesota")]
        [InlineData("http://api?filter=name" + DoubleEncoded.NOT_IN + "University of Pennsylvania" + DoubleEncoded.COMMA + "University of Minnesota", LogicalOperator.And, "name", ComparisonOperator.NotIn, "University of Pennsylvania,University of Minnesota")]
        [InlineData("http://api?filter=yearofestablishment" + DoubleEncoded.NULL, LogicalOperator.And, "yearofestablishment", ComparisonOperator.Null)]
        [InlineData("http://api?filter=yearofestablishment" + DoubleEncoded.NOT_NULL, LogicalOperator.And, "yearofestablishment", ComparisonOperator.NotNull)]
        [InlineData("http://api?filter=name" + DoubleEncoded.LIKE + "University", LogicalOperator.And, "name", ComparisonOperator.Like, "University")]
        [InlineData("http://api?filter=name" + DoubleEncoded.NOT_LIKE + "University", LogicalOperator.And, "name", ComparisonOperator.NotLike, "University")]
        public void Parse_ValidFilter_EqualToExpected(string url, LogicalOperator logicalOperator, string field, ComparisonOperator comparisonOperator, string value = null)
        {
            var queryString = _queryStringParser.Parse(typeof(SchoolOutDto), GetQuery(url));
            Assert.True(CompareFilter(queryString.QueryParams.Filter.First(), logicalOperator, field, comparisonOperator, value));
        }

        [Fact]
        public void Parse_TrimFilterFieldValues_EqualToExpected()
        {
            var queryString = _queryStringParser.Parse(typeof(SchoolOutDto), GetQuery("http://api?filter=name " + DoubleEncoded.EQUAL + " University of Pennsylvania "));
            Assert.True(CompareFilter(queryString.QueryParams.Filter.First(), LogicalOperator.And, "name", ComparisonOperator.Equal, "University of Pennsylvania"));
        }

        [Fact]
        public void Parse_FilterWithManyLogicalParts_EqualToExpected()
        {
            var queryString = _queryStringParser.Parse(typeof(SchoolOutDto),
                GetQuery("http://api?filter=name" + DoubleEncoded.NOT_EQUAL + "University" + DoubleEncoded.AND + "name" + DoubleEncoded.LIKE + "of" +
                DoubleEncoded.OR + "yearofestablishment" + DoubleEncoded.GREATER + "1891"));
            Assert.True(CompareFilter(queryString.QueryParams.Filter.ElementAt(0), LogicalOperator.And, "name", ComparisonOperator.NotEqual, "University") &&
                        CompareFilter(queryString.QueryParams.Filter.ElementAt(1), LogicalOperator.And, "name", ComparisonOperator.Like, "of") &&
                        CompareFilter(queryString.QueryParams.Filter.ElementAt(2), LogicalOperator.Or, "yearofestablishment", ComparisonOperator.Greater, "1891"));
        }

        [Theory]
        [InlineData("http://api?filter=name" + DoubleEncoded.IN + "University of Pennsylvania", LogicalOperator.And, "name", ComparisonOperator.In, "University of Pennsylvania")]
        [InlineData("http://api?filter=name" + DoubleEncoded.NOT_IN + "University of Pennsylvania", LogicalOperator.And, "name", ComparisonOperator.NotIn, "University of Pennsylvania")]
        public void Parse_FilterWithInAndNotInOperatorsWithoutComma_EqualToExpected(string url, LogicalOperator logicalOperator, string field, ComparisonOperator comparisonOperator, string value = null)
        {
            var queryString = _queryStringParser.Parse(typeof(SchoolOutDto), GetQuery(url));
            Assert.True(CompareFilter(queryString.QueryParams.Filter.First(), logicalOperator, field, comparisonOperator, value));
        }

        [Theory]
        [InlineData("http://api?filter=lessons!.books.name" + DoubleEncoded.NOT_NULL, LogicalOperator.And, "lessons!.books.name", ComparisonOperator.NotNull)]
        [InlineData("http://api?filter=lessons.books!.name" + DoubleEncoded.NOT_NULL, LogicalOperator.And, "lessons.books!.name", ComparisonOperator.NotNull)]
        public void Parse_FilterWithExclamation_EqualToExpected(string url, LogicalOperator logicalOperator, string field, ComparisonOperator comparisonOperator, string value = null)
        {
            var queryString = _queryStringParser.Parse(typeof(SchoolOutDto), GetQuery(url));
            Assert.True(CompareFilter(queryString.QueryParams.Filter.First(), logicalOperator, field, comparisonOperator, value));
        }

        [Fact]
        public void Parse_FilterWithAsterisk_Error()
        {
            Error("http://api?filter=students.*" + DoubleEncoded.EQUAL + "John", QueryStringErrorType.InvalidField);
        }

        [Theory]
        [InlineData("http://api?filter=lessons.name!" + DoubleEncoded.NOT_NULL)]
        [InlineData("http://api?filter=name!" + DoubleEncoded.NOT_NULL)]
        public void Parse_FilterWithExclamationAtNonDetail_Error(string url)
        {
            Error(url, QueryStringErrorType.InvalidField);
        }

        [Fact]
        public void Parse_FilterWithMoreThanOneExclamation_Error()
        {
            Error("http://api?filter=students!!.firstname" + DoubleEncoded.EQUAL + "Mark", QueryStringErrorType.InvalidField);
        }

        [Fact]
        public void Parse_FilterWithExclamationNotAtTheEnd_Error()
        {
            Error("http://api?filter=!students.firstname" + DoubleEncoded.EQUAL + "Mark", QueryStringErrorType.InvalidField);
        }

        [Fact]
        public void Parse_FilterWithNoOperator_Error()
        {
            Error("http://api?filter=name", QueryStringErrorType.InvalidFormat);
        }

        [Fact]
        public void Parse_FilterWithNoOperatorInOneLogicalPart_Error()
        {
            Error("http://api?filter=name" + DoubleEncoded.NOT_EQUAL + "University" + DoubleEncoded.AND + "name", QueryStringErrorType.InvalidFormat);
        }

        [Fact]
        public void Parse_FilterWithMoreThanOneOperatorsInOneLogicalPart_Error()
        {
            Error("http://api?filter=name" + DoubleEncoded.EQUAL + DoubleEncoded.LIKE + "University of Pennsylvania", QueryStringErrorType.InvalidFormat);
        }

        [Fact]
        public void Parse_FilterWithMoreThanOneInvalidLogicalParts_Error()
        {
            Error("http://api?filter=name" + DoubleEncoded.EQUAL + DoubleEncoded.NOT_EQUAL + "University" + DoubleEncoded.OR + "name" +
                DoubleEncoded.EQUAL + DoubleEncoded.LIKE + "of", QueryStringErrorType.InvalidFormat);
        }

        [Fact]
        public void Parse_MoreThanOneFiltersWithMoreThanOneInvalidLogicalParts_Errors()
        {
            var queryString = _queryStringParser.Parse(typeof(SchoolOutDto),
                GetQuery("http://api?filter=name" + DoubleEncoded.EQUAL + DoubleEncoded.NOT_EQUAL + "University" + DoubleEncoded.OR + "name" +
                DoubleEncoded.EQUAL + DoubleEncoded.NOT_EQUAL + "school,filter=name" + DoubleEncoded.EQUAL + DoubleEncoded.NOT_EQUAL + "College" +
                DoubleEncoded.OR + "name" + DoubleEncoded.EQUAL + DoubleEncoded.NOT_EQUAL + "University of Minnesota"));
            Assert.True(queryString.Errors.Where(p => p.Type == QueryStringErrorType.InvalidFormat).Count() == 2);
        }

        [Fact]
        public void Parse_FilterWithOneEmptyLogicalPart_Error()
        {
            Error("http://api?filter=name" + DoubleEncoded.EQUAL + "University of Pennsylvania" + DoubleEncoded.AND, QueryStringErrorType.InvalidFormat);
        }

        [Fact]
        public void Parse_FilterWithMoreThanOneEmptyLogicalParts_Error()
        {
            Error("http://api?filter=" + DoubleEncoded.AND + DoubleEncoded.OR, QueryStringErrorType.InvalidFormat);
        }

        [Theory]
        [InlineData("http://api?filter=name" + DoubleEncoded.IN)]
        [InlineData("http://api?filter=name" + DoubleEncoded.NOT_IN)]
        public void Parse_FilterWithInAndNotInOperatorsWithoutValues_Error(string url)
        {
            Error(url, QueryStringErrorType.InvalidFormat);
        }

        [Theory]
        [InlineData("http://api?filter=" + DoubleEncoded.NULL + "name")]
        [InlineData("http://api?filter=" + DoubleEncoded.NOT_NULL + "name")]
        public void Parse_FilterWithNullityOperatorNotAtTheEnd_Error(string url)
        {
            Error(url, QueryStringErrorType.InvalidFormat);
        }

        private IEnumerable<KeyValuePair<string, StringValues>> GetQuery(string url)
        {
            var uri = new Uri(url);
            return QueryHelpers.ParseQuery(uri.Query);
        }

        private void Error(string url, QueryStringErrorType errorType)
        {
            var queryString = _queryStringParser.Parse(typeof(SchoolOutDto), GetQuery(url));
            Assert.True(queryString.Errors.Where(p => p.Type == errorType).Count() == 1);
        }

        private bool CompareFilter(QueryLogicalFilter filter, LogicalOperator logicalOperator, string field, ComparisonOperator comparisonOperator, string value)
        {
            return filter.Operator == logicalOperator &&
                filter.Filter.Field == field &&
                filter.Filter.Operator == comparisonOperator &&
                Enumerable.SequenceEqual(filter.Filter.Values, (value != null ? value.Split(',') : new string[] { }));
        }
    }
}
