using CoreApiDirect.Options;
using Microsoft.Extensions.Options;

namespace CoreApiDirect.Tests.Options
{
    internal class CoreOptionsTests : IOptions<CoreOptions>
    {
        public CoreOptions Value => new CoreOptions();
    }
}
