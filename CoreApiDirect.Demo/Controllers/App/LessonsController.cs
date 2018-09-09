using CoreApiDirect.Controllers;
using CoreApiDirect.Demo.Dto.In.App;
using CoreApiDirect.Demo.Dto.Out.App;
using CoreApiDirect.Demo.Entities.App;

namespace CoreApiDirect.Demo.Controllers.App
{
    [ApiRoute(typeof(School), typeof(Lesson))]
    public class LessonsController : ControllerApi<string, Lesson, LessonOutDto, LessonInDto>
    {
    }
}
