using System.ComponentModel.DataAnnotations;
using CoreApiDirect.Entities;

namespace CoreApiDirect.Demo.Entities.App
{
    public class Book : Entity<int>
    {
        [SearchKey]
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }

        public string LessonId { get; set; }
        public Lesson Lesson { get; set; }
    }
}
