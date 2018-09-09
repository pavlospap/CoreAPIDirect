namespace CoreApiDirect.Flow.Steps
{
    /// <summary>
    /// Base class to perform actions before the insertion or update of an entity during an HTTP POST, PUT or PATCH request.
    /// </summary>
    /// <typeparam name="TInDto">The DTO type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public class FlowStepBeforeSave<TInDto, TEntity> : FlowStep<TInDto, TEntity>, IFlowStepBeforeSave<TInDto, TEntity>
    {
    }

    /// <summary>
    /// Base class to perform actions before the insertion or update of an entity during an HTTP POST, PUT or PATCH request.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public class FlowStepBeforeSave<TEntity> : FlowStep<TEntity>, IFlowStepBeforeSave<TEntity>
    {
    }
}
