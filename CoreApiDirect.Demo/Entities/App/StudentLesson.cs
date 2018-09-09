using CoreApiDirect.Entities;

namespace CoreApiDirect.Demo.Entities.App
{
    public class StudentLesson : Entity<int>
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public string LessonId { get; set; }
        public Lesson Lesson { get; set; }
    }
}
