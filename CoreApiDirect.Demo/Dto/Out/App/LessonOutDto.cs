using System.Collections.Generic;
using CoreApiDirect.Dto;

namespace CoreApiDirect.Demo.Dto.Out.App
{
    public class LessonOutDto : OutDto<string>
    {
        public string Name { get; set; }
        public int WeekHours { get; set; }

        public ICollection<BookOutDto> Books { get; set; } = new HashSet<BookOutDto>();
    }
}
