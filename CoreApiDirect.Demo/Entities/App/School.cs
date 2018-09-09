using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CoreApiDirect.Entities;

namespace CoreApiDirect.Demo.Entities.App
{
    public class School : Entity<int>
    {
        [SearchKey]
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }

        public int? YearOfEstablishment { get; set; }

        public ICollection<Lesson> Lessons { get; set; } = new HashSet<Lesson>();
        public ICollection<Student> Students { get; set; } = new HashSet<Student>();
    }
}
