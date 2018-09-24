using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreApiDirect.Flow;
using Microsoft.AspNetCore.Mvc;

namespace CoreApiDirect.Controllers
{
    /// <summary>
    /// The base class for every API controller using DTOs.
    /// </summary>
    /// <typeparam name="TKey">The entity ID type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TOutDto">The DTO type for the HTTP GET requests.</typeparam>
    /// <typeparam name="TInDto">The DTO type for the HTTP POST, PUT and PATCH requests.</typeparam>
    public abstract class ControllerApi<TKey, TEntity, TOutDto, TInDto> : ControllerApiBase<TKey, TEntity, TOutDto, TInDto, IFlow<TEntity, TInDto>>
        where TEntity : class, IKey<TKey>
        where TOutDto : IKey<TKey>
        where TInDto : class
    {
        internal override sealed async Task<IActionResult> PostFlow(TInDto dto, TEntity entity, Func<Task<IActionResult>> saveFunc)
        {
            return await Flow.Post(dto, entity, saveFunc);
        }

        internal override sealed async Task<IActionResult> PostFlow(IEnumerable<TInDto> dtoList, IEnumerable<TEntity> entityList, Func<Task<IActionResult>> saveFunc)
        {
            return await Flow.Post(dtoList, entityList, saveFunc);
        }

        internal override sealed async Task<IActionResult> UpdateFlow(TInDto dto, TEntity entity, Func<Task<IActionResult>> saveFunc)
        {
            return await Flow.Update(dto, entity, saveFunc);
        }

        internal override sealed async Task<IActionResult> UpdateFlow(IEnumerable<TInDto> dtoList, IEnumerable<TEntity> entityList, Func<Task<IActionResult>> saveFunc)
        {
            return await Flow.Update(dtoList, entityList, saveFunc);
        }
    }

    /// <summary>
    /// The base class for every API controller not using DTOs.
    /// </summary>
    /// <typeparam name="TKey">The entity ID type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public abstract class ControllerApi<TKey, TEntity> : ControllerApiBase<TKey, TEntity, TEntity, TEntity, IFlow<TEntity>>
        where TEntity : class, IKey<TKey>
    {
        internal override sealed Task<IActionResult> PostFlow(TEntity dto, TEntity entity, Func<Task<IActionResult>> saveFunc)
        {
            return Flow.Post(entity, saveFunc);
        }

        internal override sealed Task<IActionResult> PostFlow(IEnumerable<TEntity> dtoList, IEnumerable<TEntity> entityList, Func<Task<IActionResult>> saveFunc)
        {
            return Flow.Post(entityList, saveFunc);
        }

        internal override sealed Task<IActionResult> UpdateFlow(TEntity dto, TEntity entity, Func<Task<IActionResult>> saveFunc)
        {
            return Flow.Update(entity, saveFunc);
        }

        internal override sealed Task<IActionResult> UpdateFlow(IEnumerable<TEntity> dtoList, IEnumerable<TEntity> entityList, Func<Task<IActionResult>> saveFunc)
        {
            return Flow.Update(entityList, saveFunc);
        }
    }
}
