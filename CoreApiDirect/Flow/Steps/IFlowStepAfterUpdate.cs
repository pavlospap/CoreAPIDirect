namespace CoreApiDirect.Flow.Steps
{
    internal interface IFlowStepAfterUpdate<TInDto, TEntity> : IFlowStep<TInDto, TEntity>
    {
    }

    internal interface IFlowStepAfterUpdate<TEntity> : IFlowStep<TEntity>
    {
    }
}
