using Microsoft.AspNetCore.Mvc;

namespace CoreApiDirect.Controllers.Filters
{
    internal class ValidateDtoListAttribute : TypeFilterAttribute
    {
        public ValidateDtoListAttribute()
            : base(typeof(ValidateDtoListFilter))
        {
        }
    }
}
