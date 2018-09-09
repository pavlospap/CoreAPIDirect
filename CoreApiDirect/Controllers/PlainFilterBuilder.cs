using System.Linq;
using CoreApiDirect.Query.Operators;
using CoreApiDirect.Query.Parameters;
using CoreApiDirect.Url.Encoding;

namespace CoreApiDirect.Controllers
{
    internal class PlainFilterBuilder : IPlainFilterBuilder
    {
        public string Build(QueryLogicalFilter[] logicalFilters)
        {
            string plainFilter = "";

            for (int i = 0; i <= logicalFilters.Length - 1; i++)
            {
                var filter = logicalFilters[i].Filter;
                bool isInOrNotInFilter = filter.Operator == ComparisonOperator.In || filter.Operator == ComparisonOperator.NotIn;
                string value = isInOrNotInFilter ? string.Join(Encoded.COMMA, filter.Values) : (filter.Values.First() ?? "").ToString();

                if (i > 0)
                {
                    plainFilter += logicalFilters[i].Operator.Encoded();
                }

                plainFilter += logicalFilters[i].Filter.Field + logicalFilters[i].Filter.Operator.Encoded() + value;
            }

            return plainFilter;
        }
    }
}
