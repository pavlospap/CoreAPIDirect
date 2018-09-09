using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CoreApiDirect.Flow
{
    internal interface IFlowStep
    {
        Task<IActionResult> Execute();
    }

    internal interface IFlowStep<TInDto, TEntity> : IFlowStep
    {
        Task<IActionResult> Execute(TInDto dto, TEntity entity);
        Task<IActionResult> Execute(IEnumerable<TInDto> dtoList, IEnumerable<TEntity> entityList);
    }

    internal interface IFlowStep<TEntity> : IFlowStep
    {
        Task<IActionResult> Execute(TEntity entity);
        Task<IActionResult> Execute(IEnumerable<TEntity> entityList);
    }
}
