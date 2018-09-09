using System;
using CoreApiDirect.Demo.DataContext;
using Microsoft.EntityFrameworkCore;

namespace CoreApiDirect.Tests.DataContext
{
    internal class LogDbContextTests : LogDbContext
    {
        public LogDbContextTests(DbContextOptions<LogDbContext> options)
            : base(options)
        {
        }

        public static LogDbContextTests GetContextWithData()
        {
            var options = new DbContextOptionsBuilder<LogDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var dbContext = new LogDbContextTests(options);

            var data = new InitialData();

            dbContext.LogEvents.AddRange(KeySetter.SetKeys(data.LogEvents));
            dbContext.LogDetail.AddRange(KeySetter.SetKeys(data.LogDetail));
            dbContext.SystemInfo.AddRange(KeySetter.SetKeys(data.SystemInfo));

            dbContext.SaveChanges();

            return dbContext;
        }
    }
}
