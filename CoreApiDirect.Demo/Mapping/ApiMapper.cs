using AutoMapper;
using CoreApiDirect.Mapping;

namespace CoreApiDirect.Demo.Mapping
{
    internal class ApiMapper : IApiMapper
    {
        private readonly IMapper _mapper;

        public ApiMapper(IMapper mapper)
        {
            _mapper = mapper;
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
