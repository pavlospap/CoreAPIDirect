using System.ComponentModel.DataAnnotations;
using CoreApiDirect.Entities;

namespace CoreApiDirect.Demo.Entities.Logging
{
    public class LogEvent : Entity<int>
    {
        [SearchKey]
        [Required(ErrorMessage = nameof(RequiredAttribute))]
        [MaxLength(25)]
        [StringLength(25, ErrorMessage = nameof(StringLengthAttribute))]
        [Display(Name = nameof(Name))]
        public string Name { get; set; }

        public LogDetail LogDetail { get; set; }
        public SystemInfo SystemInfo { get; set; }
    }
}
