using System.ComponentModel.DataAnnotations;

namespace CoreApiDirect.Demo.Dto.In.App
{
    public class PhoneInDto
    {
        [Required(ErrorMessage = nameof(RequiredAttribute))]
        [StringLength(20, ErrorMessage = nameof(StringLengthAttribute))]
        [Display(Name = nameof(Number))]
        public string Number { get; set; }
    }
}
