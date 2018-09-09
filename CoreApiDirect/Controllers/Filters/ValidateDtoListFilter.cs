using CoreApiDirect.Response;

namespace CoreApiDirect.Controllers.Filters
{
    internal class ValidateDtoListFilter : ValidateDtoFilter
    {
        protected override string DtoVariableName => "dtoList";

        public ValidateDtoListFilter(
            IResponseBuilder responseBuilder,
            IModelStateResolver modelStateResolver)
            : base(responseBuilder, modelStateResolver)
        {
        }
    }
}
