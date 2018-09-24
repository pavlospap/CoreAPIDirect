using System.Linq;
using System.Threading.Tasks;
using CoreApiDirect.Demo.DataContext;
using CoreApiDirect.Demo.Entities.App;
using CoreApiDirect.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CoreApiDirect.Demo.Repositories
{
    public class SchoolsRepository : Repository<School, int, AppDbContext>, ISchoolsRepository
    {
        private readonly AppDbContext _dbContext;

        public SchoolsRepository(AppDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> GetStudentNumber(int schoolId)
        {
            var number = _dbContext.Set<School>().Include(p => p.Students).FirstOrDefault(p => p.Id == schoolId).Students.Count();
            return await Task.FromResult(number);
        }
    }
}
