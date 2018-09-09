using System.ComponentModel.DataAnnotations;

namespace CoreApiDirect.Demo.Dto.In.App
{
    public class LessonInDto
    {
        [Required(ErrorMessage = nameof(RequiredAttribute))]
        [StringLength(25, ErrorMessage = nameof(StringLengthAttribute))]
        [Display(Name = nameof(Name))]
        public string Name { get; set; }

        [Required(ErrorMessage = nameof(RequiredAttribute))]
        [Display(Name = nameof(WeekHours))]
        public int? WeekHours { get; set; }
    }
}
