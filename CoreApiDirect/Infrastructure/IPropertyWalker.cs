namespace CoreApiDirect.Infrastructure
{
    internal interface IPropertyWalker<TResult, TWalkInfo>
    {
        TResult Accept(IPropertyWalkerVisitor<TResult, TWalkInfo> visitor, TWalkInfo walkInfo);
    }
}
