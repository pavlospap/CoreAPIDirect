using System.Collections.Generic;
using CoreApiDirect.Dto;

namespace CoreApiDirect.Demo.Dto.Out.App
{
    public class ContactInfoOutDto : OutDto<int>
    {
        public string Email { get; set; }
        public string Address { get; set; }

        public ICollection<PhoneOutDto> Phones { get; set; } = new HashSet<PhoneOutDto>();
    }
}
