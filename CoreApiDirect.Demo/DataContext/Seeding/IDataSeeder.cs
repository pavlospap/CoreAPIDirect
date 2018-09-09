using System.Threading.Tasks;

namespace CoreApiDirect.Demo.DataContext.Seeding
{
    public interface IDataSeeder
    {
        Task SeedDataAsync();
    }
}
