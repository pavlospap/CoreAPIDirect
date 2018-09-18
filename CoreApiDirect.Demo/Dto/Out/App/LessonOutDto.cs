using System.Collections.Generic;

namespace CoreApiDirect.Demo.Dto.Out.App
{
    public class LessonOutDto : OutDtoBase<string>
    {
        public string Name { get; set; }
        public int WeekHours { get; set; }

        public ICollection<BookOutDto> Books { get; set; } = new HashSet<BookOutDto>();
    }
}
