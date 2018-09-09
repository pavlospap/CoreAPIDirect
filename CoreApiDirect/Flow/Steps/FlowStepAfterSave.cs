namespace CoreApiDirect.Flow.Steps
{
    /// <summary>
    /// Base class to perform actions after the insertion or update of an entity during an HTTP POST, PUT or PATCH request.
    /// </summary>
    /// <typeparam name="TInDto">The DTO type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public class FlowStepAfterSave<TInDto, TEntity> : FlowStep<TInDto, TEntity>, IFlowStepAfterSave<TInDto, TEntity>
    {
    }

    /// <summary>
    /// Base class to perform actions after the insertion or update of an entity during an HTTP POST, PUT or PATCH request.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public class FlowStepAfterSave<TEntity> : FlowStep<TEntity>, IFlowStepAfterSave<TEntity>
    {
    }
}
