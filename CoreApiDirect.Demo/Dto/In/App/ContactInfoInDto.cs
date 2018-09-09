using System.ComponentModel.DataAnnotations;

namespace CoreApiDirect.Demo.Dto.In.App
{
    public class ContactInfoInDto
    {
        [StringLength(255, ErrorMessage = nameof(StringLengthAttribute))]
        [Display(Name = nameof(Email))]
        public string Email { get; set; }

        [StringLength(255, ErrorMessage = nameof(StringLengthAttribute))]
        [Display(Name = nameof(Address))]
        public string Address { get; set; }
    }
}
