using System;
using CoreApiDirect.Demo.DataContext;
using Microsoft.EntityFrameworkCore;

namespace CoreApiDirect.Tests.DataContext
{
    internal class AppDbContextTests : AppDbContext
    {
        public AppDbContextTests(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public static AppDbContextTests GetContextWithData()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var dbContext = new AppDbContextTests(options);

            var data = new InitialData();

            dbContext.Schools.AddRange(KeySetter.SetKeys(data.Schools));
            dbContext.Lessons.AddRange(data.Lessons);
            dbContext.Books.AddRange(KeySetter.SetKeys(data.Books));
            dbContext.Students.AddRange(KeySetter.SetKeys(data.Students));
            dbContext.StudentLessons.AddRange(KeySetter.SetKeys(data.StudentLessons));
            dbContext.ContactInfo.AddRange(KeySetter.SetKeys(data.ContactInfo));
            dbContext.Phones.AddRange(KeySetter.SetKeys(data.Phones));

            dbContext.SaveChanges();

            return dbContext;
        }
    }
}
