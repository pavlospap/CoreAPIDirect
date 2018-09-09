namespace CoreApiDirect.Flow.Steps
{
    internal interface IFlowStepBeforeUpdate<TInDto, TEntity> : IFlowStep<TInDto, TEntity>
    {
    }

    internal interface IFlowStepBeforeUpdate<TEntity> : IFlowStep<TEntity>
    {
    }
}
