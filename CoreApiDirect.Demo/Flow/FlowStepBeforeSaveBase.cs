using System.Threading.Tasks;
using CoreApiDirect.Flow.Steps;
using Microsoft.AspNetCore.Mvc;

namespace CoreApiDirect.Demo.Flow
{
    public class FlowStepBeforeSaveBase<TInDto, TEntity> : FlowStepBeforeSave<TInDto, TEntity>
    {
        public override async Task<IActionResult> Execute(TInDto dto, TEntity entity)
        {
            StringPropertyTrimmer.Trim(entity);
            return await base.Execute(dto, entity);
        }
    }

    public class FlowStepBeforeSaveBase<TEntity> : FlowStepBeforeSave<TEntity>
    {
        public override async Task<IActionResult> Execute(TEntity entity)
        {
            StringPropertyTrimmer.Trim(entity);
            return await base.Execute(entity);
        }
    }
}
