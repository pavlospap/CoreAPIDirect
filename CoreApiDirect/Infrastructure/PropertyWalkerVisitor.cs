using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace CoreApiDirect.Infrastructure
{
    internal abstract class PropertyWalkerVisitor<TResult, TWalkInfo> : IPropertyWalkerVisitor<TResult, TWalkInfo>
    {
        protected readonly IServiceProvider ServiceProvider;

        public PropertyWalkerVisitor(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public abstract TResult Output(TWalkInfo walkInfo);

        public abstract void Visit(PropertyInfo property, TWalkInfo walkInfo);

        protected TNextResult NextWalk<TNextResult, TNextWalkInfo>(TNextWalkInfo walkInfo, Type walkerType, Type visitorType)
        {
            var walker = ServiceProvider.GetRequiredService(walkerType);
            var visitor = ServiceProvider.GetRequiredService(visitorType);
            var acceptMethod = walker.GetType().GetMethod("Accept");

            return (TNextResult)acceptMethod.Invoke(walker, new object[] { visitor, walkInfo });
        }
    }
}
