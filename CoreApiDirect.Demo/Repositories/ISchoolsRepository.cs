using System.Threading.Tasks;
using CoreApiDirect.Demo.Entities.App;
using CoreApiDirect.Repositories;

namespace CoreApiDirect.Demo.Repositories
{
    public interface ISchoolsRepository : IRepository<School, int>
    {
        Task<int> GetStudentCount(int schoolId);
    }
}
