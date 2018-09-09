namespace CoreApiDirect.Flow.Steps
{
    /// <summary>
    /// Base class to perform actions after the update of an entity during an HTTP PUT or PATCH request.
    /// </summary>
    /// <typeparam name="TInDto">The DTO type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public class FlowStepAfterUpdate<TInDto, TEntity> : FlowStep<TInDto, TEntity>, IFlowStepAfterUpdate<TInDto, TEntity>
    {
    }

    /// <summary>
    /// Base class to perform actions after the update of an entity during an HTTP PUT or PATCH request.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public class FlowStepAfterUpdate<TEntity> : FlowStep<TEntity>, IFlowStepAfterUpdate<TEntity>
    {
    }
}
