using System.Threading.Tasks;
using CoreApiDirect.Controllers;
using CoreApiDirect.Demo.Dto.In.App;
using CoreApiDirect.Demo.Dto.Out.App;
using CoreApiDirect.Demo.Entities.App;
using CoreApiDirect.Demo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CoreApiDirect.Demo.Controllers.App
{
    [ApiRoute(typeof(School))]
    public class SchoolsController : ControllerApi<int, School, SchoolOutDto, SchoolInDto>
    {
        private readonly ISchoolsRepository _schoolsRepository;

        public SchoolsController(ISchoolsRepository schoolsRepository)
        {
            _schoolsRepository = schoolsRepository;
        }

        [HttpGet("{id}/studentnum")]
        public async Task<IActionResult> GetStudentNumberAsync(int id)
        {
            int number = await _schoolsRepository.GetStudentNumberAsync(id);
            return Ok(ResponseBuilder.AddData(new { number }).Build());
        }
    }
}
