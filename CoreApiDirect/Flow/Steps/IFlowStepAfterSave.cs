namespace CoreApiDirect.Flow.Steps
{
    internal interface IFlowStepAfterSave<TInDto, TEntity> : IFlowStep<TInDto, TEntity>
    {
    }

    internal interface IFlowStepAfterSave<TEntity> : IFlowStep<TEntity>
    {
    }
}
