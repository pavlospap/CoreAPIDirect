using AutoMapper;
using CoreApiDirect.Demo.Dto.In.App;
using CoreApiDirect.Demo.Dto.Out.App;
using CoreApiDirect.Demo.Entities.App;
using CoreApiDirect.Demo.Entities.Logging;

namespace CoreApiDirect.Demo.Mapping
{
    internal class AppProfile : Profile
    {
        public AppProfile()
        {
            CreateMap<Book, BookOutDto>();
            CreateMap<Book, BookInDto>()
                .ReverseMap();
            CreateMap<ContactInfo, ContactInfoOutDto>();
            CreateMap<ContactInfo, ContactInfoInDto>()
                .ReverseMap();
            CreateMap<Lesson, LessonOutDto>();
            CreateMap<Lesson, LessonInDto>()
                .ReverseMap();
            CreateMap<Phone, PhoneOutDto>();
            CreateMap<Phone, PhoneInDto>()
                .ReverseMap();
            CreateMap<School, SchoolOutDto>();
            CreateMap<School, SchoolInDto>()
                .ReverseMap();
            CreateMap<Student, StudentOutDto>()
                .ForMember(p => p.FullName, options => options.ResolveUsing<FullNameResolver>());
            CreateMap<Student, StudentInDto>()
                .ReverseMap();
            CreateMap<LogEvent, LogEvent>();
            CreateMap<LogDetail, LogDetail>();
            CreateMap<SystemInfo, SystemInfo>();
        }
    }
}
