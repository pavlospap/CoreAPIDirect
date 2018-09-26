using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CoreApiDirect.Flow
{
    /// <summary>
    /// Provides functionality to perform actions during an HTTP DELETE request.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public interface IFlowDelete<TEntity>
    {
        /// <summary>
        /// Asynchronously performs actions before or after the deletion of an entity.
        /// </summary>
        /// <param name="entity">An entity object.</param>
        /// <param name="saveFunc">A function that will be used for saving.</param>
        /// <returns>Returns a Microsoft.AspNetCore.Mvc.IActionResult. If it's not null it will be used as the controller's action result.</returns>
        Task<IActionResult> DeleteAsync(TEntity entity, Func<Task<IActionResult>> saveFunc);

        /// <summary>
        /// Asynchronously performs actions before or after the deletion of an enumerable of entities.
        /// </summary>
        /// <param name="entityList">An enumerable of entity objects.</param>
        /// <param name="saveFunc">A function that will be used for saving.</param>
        /// <returns>Returns a Microsoft.AspNetCore.Mvc.IActionResult. If it's not null it will be used as the controller's action result.</returns>
        Task<IActionResult> DeleteAsync(IEnumerable<TEntity> entityList, Func<Task<IActionResult>> saveFunc);
    }
}
