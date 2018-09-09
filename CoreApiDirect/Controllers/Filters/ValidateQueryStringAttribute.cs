using Microsoft.AspNetCore.Mvc;

namespace CoreApiDirect.Controllers.Filters
{
    internal class ValidateQueryStringAttribute : TypeFilterAttribute
    {
        public ValidateQueryStringAttribute()
            : base(typeof(ValidateQueryStringFilter))
        {
        }
    }
}
