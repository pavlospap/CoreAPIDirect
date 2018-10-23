using System.Collections.Generic;
using CoreApiDirect.Controllers;
using CoreApiDirect.Query.Operators;
using CoreApiDirect.Query.Parameters;
using CoreApiDirect.Url.Encoding;
using Xunit;

namespace CoreApiDirect.Tests.Controllers
{
    public class PlainFilterBuilderTests
    {
        [Fact]
        public void Build_ValidData_PlainFilter()
        {
            var logicalFilters = new QueryLogicalFilter[]
            {
                new QueryLogicalFilter
                {
                    Operator = LogicalOperator.And,
                    Filter = new QueryComparisonFilter
                    {
                        Field = "name",
                        Operator = ComparisonOperator.In,
                        Values= new List<string>
                        {
                            "University of Pennsylvania",
                            "University of Minnesota"
                        }
                    }
                },
                new QueryLogicalFilter
                {
                    Operator = LogicalOperator.Or,
                    Filter = new QueryComparisonFilter
                    {
                        Field = "yearofestablishment",
                        Operator = ComparisonOperator.Equal,
                        Values = new List<string> { "1891" }
                    }
                }
            };

            string expectedPlainFilter = "name" + ComparisonOperator.In.Encoded() + "University of Pennsylvania" + Encoded.COMMA +
                "University of Minnesota" + LogicalOperator.Or.Encoded() + "yearofestablishment" + ComparisonOperator.Equal.Encoded() + "1891";

            Assert.Equal(expectedPlainFilter, new PlainFilterBuilder().Build(logicalFilters));
        }
    }
}
