using System.Threading.Tasks;
using CoreApiDirect.Flow.Steps;
using Microsoft.AspNetCore.Mvc;

namespace CoreApiDirect.Demo.Flow
{
    public class FlowStepBeforeSaveBase<TInDto, TEntity> : FlowStepBeforeSave<TInDto, TEntity>
    {
        public override async Task<IActionResult> ExecuteAsync(TInDto dto, TEntity entity)
        {
            StringPropertyTrimmer.Trim(entity);
            return await base.ExecuteAsync(dto, entity);
        }
    }

    public class FlowStepBeforeSaveBase<TEntity> : FlowStepBeforeSave<TEntity>
    {
        public override async Task<IActionResult> ExecuteAsync(TEntity entity)
        {
            StringPropertyTrimmer.Trim(entity);
            return await base.ExecuteAsync(entity);
        }
    }
}
