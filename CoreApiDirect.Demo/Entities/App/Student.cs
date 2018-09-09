using System;
using System.ComponentModel.DataAnnotations;
using CoreApiDirect.Entities;

namespace CoreApiDirect.Demo.Entities.App
{
    public class Student : EntityBase
    {
        [SearchKey]
        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [SearchKey]
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int SchoolId { get; set; }
        public School School { get; set; }

        // TODO : Enable Many to many
        // public ICollection<StudentLesson> StudentLessons { get; set; } = new HashSet<StudentLesson>();

        public ContactInfo ContactInfo { get; set; }
    }
}
