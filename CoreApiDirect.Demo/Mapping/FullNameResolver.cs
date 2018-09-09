using AutoMapper;
using CoreApiDirect.Demo.Dto.Out.App;
using CoreApiDirect.Demo.Entities.App;

namespace CoreApiDirect.Demo.Mapping
{
    internal class FullNameResolver : IValueResolver<Student, StudentOutDto, string>
    {
        public string Resolve(Student source, StudentOutDto destination, string destMember, ResolutionContext context)
        {
            return source.FirstName + " " + source.LastName;
        }
    }
}
