using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CoreApiDirect.Flow
{
    /// <summary>
    /// Base class to perform actions during an HTTP POST, PUT, PATCH or DELETE request.
    /// </summary>
    public abstract class FlowStep : IFlowStep
    {
        /// <summary>
        /// Asynchronously performs actions before or after the insertion, update or deletion of an entity or an enumerable of entities.
        /// </summary>
        /// <returns>Returns a Microsoft.AspNetCore.Mvc.IActionResult. If it's not null it will be used as the controller's action result.</returns>
        public async virtual Task<IActionResult> Execute()
        {
            return await Task.FromResult<IActionResult>(null);
        }
    }

    /// <summary>
    /// Base class to perform actions during an HTTP POST, PUT or PATCH request.
    /// </summary>
    /// <typeparam name="TInDto">The DTO type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public abstract class FlowStep<TInDto, TEntity> : FlowStep, IFlowStep<TInDto, TEntity>
    {
        /// <summary>
        /// Asynchronously performs actions before or after the insertion or update of an entity or an enumerable of entities.
        /// </summary>
        /// <returns>Returns a Microsoft.AspNetCore.Mvc.IActionResult. If it's not null it will be used as the controller's action result.</returns>
        public override sealed async Task<IActionResult> Execute()
        {
            return await base.Execute();
        }

        /// <summary>
        /// Asynchronously performs actions before or after the insertion or update of an entity.
        /// </summary>
        /// <param name="dto">A DTO object.</param>
        /// <param name="entity">An entity object.</param>
        /// <returns>Returns a Microsoft.AspNetCore.Mvc.IActionResult. If it's not null it will be used as the controller's action result.</returns>
        public virtual async Task<IActionResult> Execute(TInDto dto, TEntity entity)
        {
            return await Task.FromResult<IActionResult>(null);
        }

        /// <summary>
        /// Asynchronously performs actions before or after the insertion or update of an enumerable of entities.
        /// </summary>
        /// <param name="dtoList">An enumerable of DTO objects.</param>
        /// <param name="entityList">An enumerable of entity objects.</param>
        /// <returns>Returns a Microsoft.AspNetCore.Mvc.IActionResult. If it's not null it will be used as the controller's action result.</returns>
        public async Task<IActionResult> Execute(IEnumerable<TInDto> dtoList, IEnumerable<TEntity> entityList)
        {
            for (int i = 0; i <= dtoList.Count() - 1; i++)
            {
                var result = await Execute(dtoList.ElementAt(i), entityList.ElementAt(i));
                if (result != null)
                {
                    return result;
                }
            }

            return await Task.FromResult<IActionResult>(null);
        }
    }

    /// <summary>
    /// Base class to perform actions during an HTTP POST, PUT, PATCH or DELETE request.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public abstract class FlowStep<TEntity> : FlowStep, IFlowStep<TEntity>
    {
        /// <summary>
        /// Asynchronously performs actions before or after the insertion, update or deletion of an entity or an enumerable of entities.
        /// </summary>
        /// <returns>Returns a Microsoft.AspNetCore.Mvc.IActionResult. If it's not null it will be used as the controller's action result.</returns>
        public override sealed async Task<IActionResult> Execute()
        {
            return await base.Execute();
        }

        /// <summary>
        /// Asynchronously performs actions before or after the insertion, update or deletion of an entity.
        /// </summary>
        /// <param name="entity">An entity object.</param>
        /// <returns>Returns a Microsoft.AspNetCore.Mvc.IActionResult. If it's not null it will be used as the controller's action result.</returns>
        public virtual async Task<IActionResult> Execute(TEntity entity)
        {
            return await Task.FromResult<IActionResult>(null);
        }

        /// <summary>
        /// Asynchronously performs actions before or after the insertion, update or deletion of an enumerable of entities.
        /// </summary>
        /// <param name="entityList">An enumerable of entity objects.</param>
        /// <returns>Returns a Microsoft.AspNetCore.Mvc.IActionResult. If it's not null it will be used as the controller's action result.</returns>
        public async Task<IActionResult> Execute(IEnumerable<TEntity> entityList)
        {
            foreach (var entity in entityList)
            {
                var result = await Execute(entity);
                if (result != null)
                {
                    return result;
                }
            }

            return await Task.FromResult<IActionResult>(null);
        }
    }
}
