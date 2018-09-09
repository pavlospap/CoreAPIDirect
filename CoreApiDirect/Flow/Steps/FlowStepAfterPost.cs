namespace CoreApiDirect.Flow.Steps
{
    /// <summary>
    /// Base class to perform actions after the insertion of an entity during an HTTP POST request.
    /// </summary>
    /// <typeparam name="TInDto">The DTO type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public class FlowStepAfterPost<TInDto, TEntity> : FlowStep<TInDto, TEntity>, IFlowStepAfterPost<TInDto, TEntity>
    {
    }

    /// <summary>
    /// Base class to perform actions after the insertion of an entity during an HTTP POST request.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public class FlowStepAfterPost<TEntity> : FlowStep<TEntity>, IFlowStepAfterPost<TEntity>
    {
    }
}
