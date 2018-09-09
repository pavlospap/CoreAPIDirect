using System;
using System.Linq;

namespace CoreApiDirect.Flow
{
    internal class FlowStepInfo
    {
        public IFlowStep Step { get; }
        public object[] Parameters { get; }
        public Type[] ParameterTypes { get; }

        public FlowStepInfo(
            IFlowStep step,
            params object[] parameters)
        {
            Step = step;
            Parameters = parameters;
            ParameterTypes = parameters.Select(p => p.GetType()).ToArray();
        }
    }
}
