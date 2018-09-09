using System;

namespace CoreApiDirect.Demo.Dto.Out.App
{
    public class StudentOutDto : OutDtoBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public ContactInfoOutDto ContactInfo { get; set; }
    }
}
