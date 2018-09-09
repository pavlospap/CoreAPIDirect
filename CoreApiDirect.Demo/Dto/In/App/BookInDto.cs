using System.ComponentModel.DataAnnotations;

namespace CoreApiDirect.Demo.Dto.In.App
{
    public class BookInDto
    {
        [Required(ErrorMessage = nameof(RequiredAttribute))]
        [StringLength(25, ErrorMessage = nameof(StringLengthAttribute))]
        [Display(Name = nameof(Name))]
        public string Name { get; set; }
    }
}
