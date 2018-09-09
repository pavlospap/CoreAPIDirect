using System.ComponentModel.DataAnnotations;
using CoreApiDirect.Entities;

namespace CoreApiDirect.Demo.Entities.App
{
    public class Phone : Entity<int>
    {
        [SearchKey]
        [Required]
        [MaxLength(20)]
        public string Number { get; set; }

        public int ContactInfoId { get; set; }
        public ContactInfo ContactInfo { get; set; }
    }
}
