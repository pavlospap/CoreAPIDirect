using CoreApiDirect.Dto;
using CoreApiDirect.Entities;

namespace CoreApiDirect.Demo.Controllers
{
    public abstract class ControllerBase<TEntity, TOutDto, TInDto> : ControllerBaseKey<int, TEntity, TOutDto, TInDto>
        where TEntity : Entity<int>
        where TOutDto : OutDto<int>
        where TInDto : class
    {
    }
}
