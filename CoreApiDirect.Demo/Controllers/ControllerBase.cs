using CoreApiDirect.Controllers;
using CoreApiDirect.Dto;
using CoreApiDirect.Entities;

namespace CoreApiDirect.Demo.Controllers
{
    public abstract class ControllerBase<TKey, TEntity, TOutDto, TInDto> : ControllerApi<TKey, TEntity, TOutDto, TInDto>
        where TEntity : Entity<TKey>
        where TOutDto : OutDto<TKey>
        where TInDto : class
    {
    }

    public abstract class ControllerBase<TEntity, TOutDto, TInDto> : ControllerBase<int, TEntity, TOutDto, TInDto>
        where TEntity : Entity<int>
        where TOutDto : OutDto<int>
        where TInDto : class
    {
    }

    public abstract class ControllerBase<TKey, TEntity> : ControllerApi<TKey, TEntity>
    where TEntity : Entity<TKey>
    {
    }

    public abstract class ControllerBase<TEntity> : ControllerBase<int, TEntity>
        where TEntity : Entity<int>
    {
    }
}
