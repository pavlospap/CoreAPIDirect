using System.Reflection;

namespace CoreApiDirect.Infrastructure
{
    internal interface IPropertyWalkerVisitor<TResult, TWalkInfo>
    {
        TResult Output(TWalkInfo walkInfo);
        void Visit(PropertyInfo property, TWalkInfo walkInfo);
    }
}
