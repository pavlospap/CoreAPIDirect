using System.Threading.Tasks;
using CoreApiDirect.Demo.Dto.In.App;
using CoreApiDirect.Demo.Entities.App;
using CoreApiDirect.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace CoreApiDirect.Demo.Flow
{
    public class SchoolFlowStepBeforeSave : FlowStepBeforeSaveBase<SchoolInDto, School>
    {
        private readonly IResponseBuilder _responseBuilder;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public SchoolFlowStepBeforeSave(
            IResponseBuilder responseBuilder,
            IStringLocalizer<SharedResource> localizer)
        {
            _responseBuilder = responseBuilder;
            _localizer = localizer;
        }

        public override async Task<IActionResult> ExecuteAsync(SchoolInDto dto, School entity)
        {
            if (entity.YearOfEstablishment != null && entity.YearOfEstablishment < 1700)
            {
                return new BadRequestObjectResult(_responseBuilder.AddError(_localizer["YearOfEstablishmentError"]).Build());
            }

            return await base.ExecuteAsync(dto, entity);
        }
    }
}
