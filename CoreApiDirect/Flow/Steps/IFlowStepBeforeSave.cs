namespace CoreApiDirect.Flow.Steps
{
    internal interface IFlowStepBeforeSave<TInDto, TEntity> : IFlowStep<TInDto, TEntity>
    {
    }

    internal interface IFlowStepBeforeSave<TEntity> : IFlowStep<TEntity>
    {
    }
}
