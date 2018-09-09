namespace CoreApiDirect.Flow.Steps
{
    /// <summary>
    /// Base class to perform actions before the deletion of an entity during an HTTP DELETE request.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public class FlowStepBeforeDelete<TEntity> : FlowStep<TEntity>, IFlowStepBeforeDelete<TEntity>
    {
    }
}
