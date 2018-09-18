using CoreApiDirect.Dto;

namespace CoreApiDirect.Demo.Dto.Out
{
    public abstract class OutDtoBase<TKey> : OutDto<TKey>
    {
        public string Notes { get; set; }
    }

    public abstract class OutDtoBase : OutDtoBase<int>
    {
    }
}
