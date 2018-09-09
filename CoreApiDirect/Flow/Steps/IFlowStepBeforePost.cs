namespace CoreApiDirect.Flow.Steps
{
    internal interface IFlowStepBeforePost<TInDto, TEntity> : IFlowStep<TInDto, TEntity>
    {
    }

    internal interface IFlowStepBeforePost<TEntity> : IFlowStep<TEntity>
    {
    }
}
