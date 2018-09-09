namespace CoreApiDirect.Flow.Steps
{
    internal interface IFlowStepAfterPost<TInDto, TEntity> : IFlowStep<TInDto, TEntity>
    {
    }

    internal interface IFlowStepAfterPost<TEntity> : IFlowStep<TEntity>
    {
    }
}
