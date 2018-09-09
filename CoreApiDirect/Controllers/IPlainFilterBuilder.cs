using CoreApiDirect.Query.Parameters;

namespace CoreApiDirect.Controllers
{
    internal interface IPlainFilterBuilder
    {
        string Build(QueryLogicalFilter[] logicalFilters);
    }
}
