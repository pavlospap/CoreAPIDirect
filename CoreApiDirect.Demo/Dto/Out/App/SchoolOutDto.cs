using System.Collections.Generic;
using CoreApiDirect.Dto;

namespace CoreApiDirect.Demo.Dto.Out.App
{
    public class SchoolOutDto : OutDto<int>
    {
        public string Name { get; set; }
        public int? YearOfEstablishment { get; set; }

        public ICollection<LessonOutDto> Lessons { get; set; } = new HashSet<LessonOutDto>();
        public ICollection<StudentOutDto> Students { get; set; } = new HashSet<StudentOutDto>();
    }
}
