namespace CoreApiDirect.Flow.Steps
{
    /// <summary>
    /// Base class to perform actions before the update of an entity during an HTTP PUT or PATCH request.
    /// </summary>
    /// <typeparam name="TInDto">The DTO type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public class FlowStepBeforeUpdate<TInDto, TEntity> : FlowStep<TInDto, TEntity>, IFlowStepBeforeUpdate<TInDto, TEntity>
    {
    }

    /// <summary>
    /// Base class to perform actions before the update of an entity during an HTTP PUT or PATCH request.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public class FlowStepBeforeUpdate<TEntity> : FlowStep<TEntity>, IFlowStepBeforeUpdate<TEntity>
    {
    }
}
