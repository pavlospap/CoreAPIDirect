using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CoreApiDirect.Flow.Steps
{
    /// <summary>
    /// Base class to perform actions before the insertion of an entity during an HTTP POST request.
    /// </summary>
    /// <typeparam name="TInDto">The DTO type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public class FlowStepBeforePost<TInDto, TEntity> : FlowStep<TInDto, TEntity>, IFlowStepBeforePost<TInDto, TEntity>
    {
        private readonly IForeignKeysResolver _foreignKeysResolver;

        /// <summary>
        /// Initializes a new instance of the CoreApiDirect.Flow.Steps.FlowStepBeforePost class.
        /// </summary>
        /// <param name="foreignKeysResolver">The service to resolve foreign keys.</param>
        public FlowStepBeforePost(IForeignKeysResolver foreignKeysResolver)
        {
            _foreignKeysResolver = foreignKeysResolver;
        }

        /// <summary>
        /// Asynchronously performs actions before the insertion of an entity.
        /// </summary>
        /// <param name="dto">A DTO object.</param>
        /// <param name="entity">An entity object.</param>
        /// <returns>Returns a Microsoft.AspNetCore.Mvc.IActionResult. If it's not null it will be used as the controller's action result.</returns>
        public override Task<IActionResult> Execute(TInDto dto, TEntity entity)
        {
            _foreignKeysResolver.FillForeignKeysFromRoute(entity);
            return base.Execute(dto, entity);
        }
    }

    /// <summary>
    /// Base class to perform actions before the insertion of an entity during an HTTP POST request.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public class FlowStepBeforePost<TEntity> : FlowStep<TEntity>, IFlowStepBeforePost<TEntity>
    {
        private readonly IForeignKeysResolver _foreignKeysResolver;

        /// <summary>
        /// Initializes a new instance of the CoreApiDirect.Flow.Steps.FlowStepBeforePost class.
        /// </summary>
        /// <param name="foreignKeysResolver">The service to resolve foreign keys.</param>
        public FlowStepBeforePost(IForeignKeysResolver foreignKeysResolver)
        {
            _foreignKeysResolver = foreignKeysResolver;
        }

        /// <summary>
        /// Asynchronously performs actions before the insertion of an entity.
        /// </summary>
        /// <param name="entity">An entity object.</param>
        /// <returns>Returns a Microsoft.AspNetCore.Mvc.IActionResult. If it's not null it will be used as the controller's action result.</returns>
        public override Task<IActionResult> Execute(TEntity entity)
        {
            _foreignKeysResolver.FillForeignKeysFromRoute(entity);
            return base.Execute(entity);
        }
    }
}
