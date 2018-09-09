using CoreApiDirect.Demo.Entities.App;
using Microsoft.EntityFrameworkCore;

namespace CoreApiDirect.Demo.DataContext
{
    public class AppDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<ContactInfo> ContactInfo { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentLesson> StudentLessons { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}
