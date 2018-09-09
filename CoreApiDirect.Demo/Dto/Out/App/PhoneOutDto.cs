using CoreApiDirect.Dto;

namespace CoreApiDirect.Demo.Dto.Out.App
{
    public class PhoneOutDto : OutDto<int>
    {
        public string Number { get; set; }
    }
}
