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

        public async Task<int> GetStudentNumberAsync(int schoolId)
        {
            var school = await _dbContext.Set<School>().Include(p => p.Students).FirstOrDefaultAsync(p => p.Id == schoolId);
            return await Task.FromResult(school.Students.Count());
        }
    }
}
