using CoreApiDirect.Dto;

namespace CoreApiDirect.Demo.Dto.Out.App
{
    public class BookOutDto : OutDto<int>
    {
        public string Name { get; set; }
    }
}
