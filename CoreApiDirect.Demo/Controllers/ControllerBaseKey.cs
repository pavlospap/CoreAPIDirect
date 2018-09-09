using CoreApiDirect.Controllers;
using CoreApiDirect.Dto;
using CoreApiDirect.Entities;

namespace CoreApiDirect.Demo.Controllers
{
    public abstract class ControllerBaseKey<TKey, TEntity, TOutDto, TInDto> : ControllerApi<TKey, TEntity, TOutDto, TInDto>
        where TEntity : Entity<TKey>
        where TOutDto : OutDto<TKey>
        where TInDto : class
    {
    }
}
