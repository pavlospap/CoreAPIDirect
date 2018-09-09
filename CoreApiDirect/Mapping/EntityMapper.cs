using AutoMapper;
using CoreApiDirect.Mapping.Configuration;

namespace CoreApiDirect.Mapping
{
    internal class EntityMapper<TEntity> : IEntityMapper<TEntity>
    {
        private readonly IMapper _mapper;

        public EntityMapper(IEntityMapperConfigurator mapperConfigurator)
        {
            _mapper = new Mapper(new MapperConfiguration(config =>
            {
                mapperConfigurator.Configure<TEntity>(config);
            }));
        }

        public TDestination Map<TDestination>(object source)
        {
            return _mapper.Map<TDestination>(source);
        }

        public void Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            _mapper.Map(source, destination);
        }
    }
}
