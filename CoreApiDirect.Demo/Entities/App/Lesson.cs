using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CoreApiDirect.Entities;

namespace CoreApiDirect.Demo.Entities.App
{
    public class Lesson : EntityBase<string>
    {
        [SearchKey]
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }

        public int WeekHours { get; set; }

        public int SchoolId { get; set; }
        public School School { get; set; }

        public ICollection<Book> Books { get; set; } = new HashSet<Book>();
        // TODO : Enable Many to many
        // public ICollection<StudentLesson> StudentLessons { get; set; } = new HashSet<StudentLesson>();

        public Lesson()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
