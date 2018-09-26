using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreApiDirect.Flow.Steps;
using Microsoft.AspNetCore.Mvc;

namespace CoreApiDirect.Flow
{
    internal class FlowDelete<TEntity> : IFlowDelete<TEntity>
    {
        private readonly IFlowStepBeforeDelete<TEntity> _beforeDelete;
        private readonly IFlowStepAfterDelete<TEntity> _afterDelete;

        public FlowDelete(
            IFlowStepBeforeDelete<TEntity> beforeDelete,
            IFlowStepAfterDelete<TEntity> afterDelete)
        {
            _beforeDelete = beforeDelete;
            _afterDelete = afterDelete;
        }

        public async Task<IActionResult> DeleteAsync(TEntity entity, Func<Task<IActionResult>> saveFunc)
        {
            return await ProcessFlowAsync(new List<FlowStepInfo>() {
                new FlowStepInfo(_beforeDelete, entity),
                new FlowStepInfo(new SaveStep(saveFunc)),
                new FlowStepInfo(_afterDelete, entity)
            });
        }

        public async Task<IActionResult> DeleteAsync(IEnumerable<TEntity> entityList, Func<Task<IActionResult>> saveFunc)
        {
            return await ProcessFlowAsync(new List<FlowStepInfo>() {
                new FlowStepInfo(_beforeDelete, entityList),
                new FlowStepInfo(new SaveStep(saveFunc)),
                new FlowStepInfo(_afterDelete, entityList)
            });
        }

        protected async Task<IActionResult> ProcessFlowAsync(List<FlowStepInfo> flowStepInfoList)
        {
            foreach (var flowStepInfo in flowStepInfoList)
            {
                var executeMethod = flowStepInfo.Step.GetType().GetMethod("ExecuteAsync", flowStepInfo.ParameterTypes);
                var result = await (Task<IActionResult>)executeMethod.Invoke(flowStepInfo.Step, flowStepInfo.Parameters);
                if (result != null)
                {
                    return result;
                }
            }

            return await Task.FromResult<IActionResult>(null);
        }
    }
}
