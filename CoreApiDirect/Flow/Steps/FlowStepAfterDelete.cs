namespace CoreApiDirect.Flow.Steps
{
    /// <summary>
    /// Base class to perform actions after the deletion of an entity during an HTTP DELETE request.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public class FlowStepAfterDelete<TEntity> : FlowStep<TEntity>, IFlowStepAfterDelete<TEntity>
    {
    }
}
