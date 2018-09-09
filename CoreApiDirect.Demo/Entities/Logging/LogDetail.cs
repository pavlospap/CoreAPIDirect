using System;
using System.ComponentModel.DataAnnotations;
using CoreApiDirect.Entities;

namespace CoreApiDirect.Demo.Entities.Logging
{
    [SingularUrl]
    public class LogDetail : Entity<int>
    {
        public DateTime LogDate { get; set; }

        [SearchKey]
        [Required(ErrorMessage = nameof(RequiredAttribute))]
        [MaxLength(255)]
        [StringLength(255, ErrorMessage = nameof(StringLengthAttribute))]
        [Display(Name = nameof(Text))]
        public string Text { get; set; }

        public int LogEventId { get; set; }
        public LogEvent LogEvent { get; set; }
    }
}
