using System.ComponentModel.DataAnnotations;
using CoreApiDirect.Entities;

namespace CoreApiDirect.Demo.Entities.Logging
{
    [SingularUrl]
    public class SystemInfo : Entity<int>
    {
        [SearchKey]
        [Required(ErrorMessage = nameof(RequiredAttribute))]
        [MaxLength(25)]
        [StringLength(25, ErrorMessage = nameof(StringLengthAttribute))]
        [Display(Name = nameof(OS))]
        public string OS { get; set; }

        [SearchKey]
        [Required(ErrorMessage = nameof(RequiredAttribute))]
        [MaxLength(10)]
        [StringLength(10, ErrorMessage = nameof(StringLengthAttribute))]
        [Display(Name = nameof(OSVersion))]
        public string OSVersion { get; set; }

        public int LogEventId { get; set; }
        public LogEvent LogEvent { get; set; }
    }
}
