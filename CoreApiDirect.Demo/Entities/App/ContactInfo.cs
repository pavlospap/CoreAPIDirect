using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CoreApiDirect.Entities;

namespace CoreApiDirect.Demo.Entities.App
{
    [SingularUrl]
    public class ContactInfo : Entity<int>
    {
        [SearchKey]
        [MaxLength(255)]
        public string Email { get; set; }

        [SearchKey]
        [MaxLength(255)]
        public string Address { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public ICollection<Phone> Phones { get; set; } = new HashSet<Phone>();
    }
}
