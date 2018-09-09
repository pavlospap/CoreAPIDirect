using System.Threading.Tasks;
using CoreApiDirect.Controllers;
using CoreApiDirect.Demo.Dto.In.App;
using CoreApiDirect.Demo.Dto.Out.App;
using CoreApiDirect.Demo.Entities.App;
using CoreApiDirect.Demo.Repositories;
using CoreApiDirect.Response;
using Microsoft.AspNetCore.Mvc;

namespace CoreApiDirect.Demo.Controllers.App
{
    [ApiRoute(typeof(School))]
    public class SchoolsController : ControllerApi<int, School, SchoolOutDto, SchoolInDto>
    {
        private readonly ISchoolsRepository _schoolsRepository;
        private readonly IResponseBuilder _responseBuilder;

        public SchoolsController(
            ISchoolsRepository schoolsRepository,
            IResponseBuilder responseBuilder)
        {
            _schoolsRepository = schoolsRepository;
            _responseBuilder = responseBuilder;
        }

        [HttpGet("{id}/studentnum")]
        public async Task<IActionResult> GetStudentNumber(int id)
        {
            int count = await _schoolsRepository.GetStudentCount(id);
            return Ok(_responseBuilder.AddData(new { count }).Build());
        }
    }
}
