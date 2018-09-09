using System;
using System.ComponentModel.DataAnnotations;

namespace CoreApiDirect.Demo.Dto.In.App
{
    public class StudentInDto
    {
        [Required(ErrorMessage = nameof(RequiredAttribute))]
        [StringLength(20, ErrorMessage = nameof(StringLengthAttribute))]
        [Display(Name = nameof(FirstName))]
        public string FirstName { get; set; }

        [Required(ErrorMessage = nameof(RequiredAttribute))]
        [StringLength(50, ErrorMessage = nameof(StringLengthAttribute))]
        [Display(Name = nameof(LastName))]
        public string LastName { get; set; }

        [Required(ErrorMessage = nameof(RequiredAttribute))]
        [Display(Name = nameof(DateOfBirth))]
        public DateTime? DateOfBirth { get; set; }
    }
}
