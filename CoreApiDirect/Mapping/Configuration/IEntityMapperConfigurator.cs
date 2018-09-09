using AutoMapper;

namespace CoreApiDirect.Mapping.Configuration
{
    internal interface IEntityMapperConfigurator
    {
        void Configure<TEntity>(IMapperConfigurationExpression config);
    }
}
