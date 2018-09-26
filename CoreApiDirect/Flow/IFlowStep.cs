using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CoreApiDirect.Flow
{
    internal interface IFlowStep
    {
        Task<IActionResult> ExecuteAsync();
    }

    internal interface IFlowStep<TInDto, TEntity> : IFlowStep
    {
        Task<IActionResult> ExecuteAsync(TInDto dto, TEntity entity);
        Task<IActionResult> ExecuteAsync(IEnumerable<TInDto> dtoList, IEnumerable<TEntity> entityList);
    }

    internal interface IFlowStep<TEntity> : IFlowStep
    {
        Task<IActionResult> ExecuteAsync(TEntity entity);
        Task<IActionResult> ExecuteAsync(IEnumerable<TEntity> entityList);
    }
}
