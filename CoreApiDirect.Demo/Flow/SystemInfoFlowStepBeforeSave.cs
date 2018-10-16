using System.Threading.Tasks;
using CoreApiDirect.Demo.Entities.Logging;
using CoreApiDirect.Flow.Steps;
using Microsoft.AspNetCore.Mvc;

namespace CoreApiDirect.Demo.Flow
{
    public class SystemInfoFlowStepBeforeSave : FlowStepBeforeSaveBase<SystemInfo>
    {
        public override async Task<IActionResult> ExecuteAsync(SystemInfo entity)
        {
            entity.OS = entity.OS.ToUpper();
            return await base.ExecuteAsync(entity);
        }
    }
}
