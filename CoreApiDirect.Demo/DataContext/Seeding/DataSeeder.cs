using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreApiDirect.Demo.Entities.App;
using CoreApiDirect.Demo.Entities.Logging;
using CoreApiDirect.Repositories;

namespace CoreApiDirect.Demo.DataContext.Seeding
{
    public class DataSeeder : IDataSeeder
    {
        private readonly IRepository<Book, int> _booksRepository;
        private readonly IRepository<ContactInfo, int> _contactInfoRepository;
        private readonly IRepository<Lesson, string> _lessonsRepository;
        private readonly IRepository<Phone, int> _phonesRepository;
        private readonly IRepository<School, int> _schoolsRepository;
        private readonly IRepository<Student, int> _studentsRepository;
        private readonly IRepository<StudentLesson, int> _studentLessonssRepository;
        private readonly IRepository<LogEvent, int> _logEventsRepository;
        private readonly IRepository<LogDetail, int> _logDetailRepository;
        private readonly IRepository<SystemInfo, int> _systemInfoRepository;

        public DataSeeder(
            IRepository<Book, int> booksRepository,
            IRepository<ContactInfo, int> contactInfoRepository,
            IRepository<Lesson, string> lessonsRepository,
            IRepository<Phone, int> phonesRepository,
            IRepository<School, int> schoolsRepository,
            IRepository<Student, int> studentsRepository,
            IRepository<StudentLesson, int> studentLessonssRepository,
            IRepository<LogEvent, int> logEventsRepository,
            IRepository<LogDetail, int> logDetailRepository,
            IRepository<SystemInfo, int> systemInfoRepository)
        {
            _booksRepository = booksRepository;
            _contactInfoRepository = contactInfoRepository;
            _lessonsRepository = lessonsRepository;
            _phonesRepository = phonesRepository;
            _schoolsRepository = schoolsRepository;
            _studentsRepository = studentsRepository;
            _studentLessonssRepository = studentLessonssRepository;
            _logEventsRepository = logEventsRepository;
            _logDetailRepository = logDetailRepository;
            _systemInfoRepository = systemInfoRepository;
        }

        public async Task SeedDataAsync()
        {
            if (_schoolsRepository.Query.Any())
            {
                return;
            }

            var data = new InitialData();

            await SaveAsync(_schoolsRepository, data.Schools);

            _lessonsRepository.AddRange(data.Lessons);
            await _lessonsRepository.SaveAsync();

            await SaveAsync(_booksRepository, data.Books);
            await SaveAsync(_studentsRepository, data.Students);
            await SaveAsync(_studentLessonssRepository, data.StudentLessons);
            await SaveAsync(_contactInfoRepository, data.ContactInfo);
            await SaveAsync(_phonesRepository, data.Phones);
            await SaveAsync(_logEventsRepository, data.LogEvents);
            await SaveAsync(_logDetailRepository, data.LogDetail);
            await SaveAsync(_systemInfoRepository, data.SystemInfo);
        }

        private async Task SaveAsync<TEntity, TKey>(IRepository<TEntity, TKey> repository, IEnumerable<TEntity> items)
        {
            // The following is slow but ensures that the records with an int Id type won't get random Ids which could happen with AddRange()
            foreach (var item in items)
            {
                repository.Add(item);
                await repository.SaveAsync();
            }
        }
    }
}
