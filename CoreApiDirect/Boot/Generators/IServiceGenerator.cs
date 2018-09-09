using Microsoft.Extensions.DependencyInjection;

namespace CoreApiDirect.Boot.Generators
{
    internal interface IServiceGenerator
    {
        IServiceCollection Generate(IServiceCollection services);
    }
}
