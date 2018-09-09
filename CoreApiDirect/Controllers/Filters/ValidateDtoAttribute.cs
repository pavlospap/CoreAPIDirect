using Microsoft.AspNetCore.Mvc;

namespace CoreApiDirect.Controllers.Filters
{
    internal class ValidateDtoAttribute : TypeFilterAttribute
    {
        public ValidateDtoAttribute()
            : base(typeof(ValidateDtoFilter))
        {
        }
    }
}
