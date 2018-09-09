using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CoreApiDirect.Flow
{
    /// <summary>
    /// Provides functionality to perform actions during an HTTP POST, PUT, PATCH or DELETE request.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInDto">The DTO type.</typeparam>
    public interface IFlow<TEntity, TInDto> : IFlowDelete<TEntity>
    {
        /// <summary>
        /// Asynchronously performs actions before or after the insertion of an entity.
        /// </summary>
        /// <param name="dto">A DTO object.</param>
        /// <param name="entity">An entity object.</param>
        /// <param name="saveFunc">A function that will be used for saving.</param>
        /// <returns>Returns a Microsoft.AspNetCore.Mvc.IActionResult. If it's not null it will be used as the controller's action result.</returns>
        Task<IActionResult> Post(TInDto dto, TEntity entity, Func<Task<IActionResult>> saveFunc);

        /// <summary>
        /// Asynchronously performs actions before or after the insertion of an enumerable of entities.
        /// </summary>
        /// <param name="dtoList">An enumerable of DTO objects.</param>
        /// <param name="entityList">An enumerable of entity objects.</param>
        /// <param name="saveFunc">A function that will be used for saving.</param>
        /// <returns>Returns a Microsoft.AspNetCore.Mvc.IActionResult. If it's not null it will be used as the controller's action result.</returns>
        Task<IActionResult> Post(IEnumerable<TInDto> dtoList, IEnumerable<TEntity> entityList, Func<Task<IActionResult>> saveFunc);

        /// <summary>
        /// Asynchronously performs actions before or after the update of an entity.
        /// </summary>
        /// <param name="dto">A DTO object.</param>
        /// <param name="entity">An entity object.</param>
        /// <param name="saveFunc">A function that will be used for saving.</param>
        /// <returns>Returns a Microsoft.AspNetCore.Mvc.IActionResult. If it's not null it will be used as the controller's action result.</returns>
        Task<IActionResult> Update(TInDto dto, TEntity entity, Func<Task<IActionResult>> saveFunc);

        /// <summary>
        /// Asynchronously performs actions before or after the update of an enumerable of entities.
        /// </summary>
        /// <param name="dtoList">An enumerable of DTO objects.</param>
        /// <param name="entityList">An enumerable of entity objects.</param>
        /// <param name="saveFunc">A function that will be used for saving.</param>
        /// <returns>Returns a Microsoft.AspNetCore.Mvc.IActionResult. If it's not null it will be used as the controller's action result.</returns>
        Task<IActionResult> Update(IEnumerable<TInDto> dtoList, IEnumerable<TEntity> entityList, Func<Task<IActionResult>> saveFunc);
    }

    /// <summary>
    /// Provides functionality to perform actions during an HTTP POST, PUT, PATCH or DELETE request.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public interface IFlow<TEntity> : IFlowDelete<TEntity>
    {
        /// <summary>
        /// Asynchronously performs actions before or after the insertion of an entity.
        /// </summary>
        /// <param name="entity">An entity object.</param>
        /// <param name="saveFunc">A function that will be used for saving.</param>
        /// <returns>Returns a Microsoft.AspNetCore.Mvc.IActionResult. If it's not null it will be used as the controller's action result.</returns>
        Task<IActionResult> Post(TEntity entity, Func<Task<IActionResult>> saveFunc);

        /// <summary>
        /// Asynchronously performs actions before or after the insertion of an enumerable of entities.
        /// </summary>
        /// <param name="entityList">An enumerable of entity objects.</param>
        /// <param name="saveFunc">A function that will be used for saving.</param>
        /// <returns>Returns a Microsoft.AspNetCore.Mvc.IActionResult. If it's not null it will be used as the controller's action result.</returns>
        Task<IActionResult> Post(IEnumerable<TEntity> entityList, Func<Task<IActionResult>> saveFunc);

        /// <summary>
        /// Asynchronously performs actions before or after the update of an entity.
        /// </summary>
        /// <param name="entity">An entity object.</param>
        /// <param name="saveFunc">A function that will be used for saving.</param>
        /// <returns>Returns a Microsoft.AspNetCore.Mvc.IActionResult. If it's not null it will be used as the controller's action result.</returns>
        Task<IActionResult> Update(TEntity entity, Func<Task<IActionResult>> saveFunc);

        /// <summary>
        /// Asynchronously performs actions before or after the update of an enumerable of entities.
        /// </summary>
        /// <param name="entityList">An enumerable of entity objects.</param>
        /// <param name="saveFunc">A function that will be used for saving.</param>
        /// <returns>Returns a Microsoft.AspNetCore.Mvc.IActionResult. If it's not null it will be used as the controller's action result.</returns>
        Task<IActionResult> Update(IEnumerable<TEntity> entityList, Func<Task<IActionResult>> saveFunc);
    }
}
