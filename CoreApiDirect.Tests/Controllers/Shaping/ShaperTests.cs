using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using AutoMapper;
using CoreApiDirect.Base;
using CoreApiDirect.Controllers;
using CoreApiDirect.Controllers.Shaping;
using CoreApiDirect.Demo.Dto.Out.App;
using CoreApiDirect.Demo.Mapping;
using CoreApiDirect.Options;
using CoreApiDirect.Query.Parameters;
using CoreApiDirect.Tests.DataContext;
using CoreApiDirect.Tests.Options;
using CoreApiDirect.Url;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace CoreApiDirect.Tests.Controllers.Shaping
{
    public class ShaperTests
    {
        private readonly IShaper _shaper;
        private readonly SchoolOutDto _school;

        public ShaperTests()
        {
            var services = new ServiceCollection();
            services.AddTransient<IShapePropertyWalker, ShapePropertyWalker>();
            services.AddTransient<IShapePropertyWalkerVisitor, ShapePropertyWalkerVisitor>();
            services.AddSingleton<IPropertyProvider, PropertyProvider>();
            services.AddSingleton<IFieldNameResolver, FieldNameResolver>();
            services.AddSingleton<IOptions<MvcJsonOptions>, MvcJsonOptionsTests>();

            var walker = new ShapePropertyWalker(new PropertyProvider());
            var visitor = new ShapePropertyWalkerVisitor(services.BuildServiceProvider(), new FieldNameResolver(new MvcJsonOptionsTests()));
            _shaper = new Shaper(walker, visitor);

            var mapper = new Mapper(new MapperConfiguration(config => config.AddProfile(new AppProfile())));
            _school = mapper.DefaultContext.Mapper.Map<SchoolOutDto>(AppDbContextTests.GetContextWithData().Schools.FirstOrDefault());
        }

        [Fact]
        public void Shape_ByFields_ShapedObject()
        {
            var queryString = new QueryString(new CoreOptions())
            {
                QueryParams = new QueryParams
                {
                    Fields = new List<string>() {
                        "name",
                        "yearofestablishment",
                        "lessons.name",
                        "lessons.books.*",
                        "students.firstname",
                        "students.lastname",
                        "students.contactinfo.*"
                    }
                }
            };

            Assert.True(Compare(_shaper.Shape(_school, queryString), ExpectedShapedByFields(_school)));
        }

        [Fact]
        public void Shape_ByRelatedDataLevel0_ShapedObject()
        {
            var queryString = new QueryString(new CoreOptions())
            {
                RelatedDataLevel = 0
            };

            Assert.True(Compare(_shaper.Shape(_school, queryString), ExpectedShapedByRelatedDataLevel0(_school)));
        }

        [Fact]
        public void Shape_ByRelatedDataLevel1_ShapedObject()
        {
            var queryString = new QueryString(new CoreOptions())
            {
                RelatedDataLevel = 1
            };

            Assert.True(Compare(_shaper.Shape(_school, queryString), ExpectedShapedByRelatedDataLevel1(_school)));
        }

        [Fact]
        public void Shape_SelectAllFields_ShapedObject()
        {
            var queryString = new QueryString(new CoreOptions())
            {
                QueryParams = new QueryParams
                {
                    Fields = new List<string>() { "*" }
                }
            };

            Assert.True(Compare(_shaper.Shape(_school, queryString), ExpectedShapedAll(_school)));
        }

        private bool Compare(ExpandoObject shaped, ExpandoObject expected)
        {
            var dictShaped = (IDictionary<string, object>)shaped;
            var dictExpected = (IDictionary<string, object>)expected;

            if (dictShaped.Count != dictExpected.Count)
            {
                return false;
            }

            foreach (var key in dictShaped.Keys)
            {
                if (dictShaped[key] is IEnumerable<ExpandoObject> listShaped)
                {
                    if (!(dictExpected[key] is IEnumerable<ExpandoObject> listExpected) ||
                        listShaped.Count() != listExpected.Count())
                    {
                        return false;
                    }

                    for (int i = 0; i <= listShaped.Count() - 1; i++)
                    {
                        if (!Compare(listShaped.ElementAt(i), listExpected.ElementAt(i)))
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    if (Convert.ToString(dictShaped[key]) != Convert.ToString(dictExpected[key]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private ExpandoObject ExpectedShapedByFields(SchoolOutDto school)
        {
            var shapedSchool = new ExpandoObject();
            var dictSchool = (IDictionary<string, object>)shapedSchool;

            dictSchool[nameof(SchoolOutDto.Name).Camelize()] = school.Name;
            dictSchool[nameof(SchoolOutDto.YearOfEstablishment).Camelize()] = school.YearOfEstablishment;

            var shapedLessons = new List<ExpandoObject>();

            foreach (var lesson in school.Lessons)
            {
                var shapedLesson = CreateObject(lesson, nameof(LessonOutDto.Name));
                var dictLesson = (IDictionary<string, object>)shapedLesson;

                dictLesson[nameof(LessonOutDto.Books).Camelize()] = CreateDetail(lesson.Books, nameof(BookOutDto.Id), nameof(BookOutDto.Name));

                shapedLessons.Add(shapedLesson);
            }

            dictSchool[nameof(SchoolOutDto.Lessons).Camelize()] = shapedLessons;

            var shapedStudents = new List<ExpandoObject>();
            foreach (var student in school.Students)
            {
                var shapedStudent = CreateObject(student, nameof(StudentOutDto.FirstName), nameof(StudentOutDto.LastName));
                var dictStudent = (IDictionary<string, object>)shapedStudent;

                var shapedContactInfo = CreateObject(student.ContactInfo, nameof(ContactInfoOutDto.Id), nameof(ContactInfoOutDto.Email), nameof(ContactInfoOutDto.Address));

                if (student.ContactInfo != null)
                {
                    var dictContactInfo = (IDictionary<string, object>)shapedContactInfo;
                    foreach (var phone in student.ContactInfo.Phones)
                    {
                        dictContactInfo[nameof(ContactInfoOutDto.Phones).Camelize()] = CreateDetail(student.ContactInfo.Phones, nameof(PhoneOutDto.Id), nameof(PhoneOutDto.Number));
                    }
                }

                dictStudent[nameof(StudentOutDto.ContactInfo).Camelize()] = shapedContactInfo;

                shapedStudents.Add(shapedStudent);
            }

            dictSchool[nameof(SchoolOutDto.Students).Camelize()] = shapedStudents;

            return shapedSchool;
        }

        private IEnumerable<ExpandoObject> CreateDetail(IEnumerable<object> objects, params string[] properties)
        {
            var shapedObjects = new List<ExpandoObject>();

            foreach (var obj in objects)
            {
                shapedObjects.Add(CreateObject(obj, properties));
            }

            return shapedObjects;
        }

        private ExpandoObject CreateObject(object obj, params string[] properties)
        {
            if (obj == null)
            {
                return null;
            }

            var shaped = new ExpandoObject();
            var dict = (IDictionary<string, object>)shaped;

            foreach (var property in properties)
            {
                dict[property.Camelize()] = obj.GetPropertyValue(property);
            }

            return shaped;
        }

        private ExpandoObject ExpectedShapedByRelatedDataLevel0(SchoolOutDto school)
        {
            var shapedSchool = new ExpandoObject();
            var dictSchool = (IDictionary<string, object>)shapedSchool;

            dictSchool[nameof(SchoolOutDto.Id).Camelize()] = school.Id;
            dictSchool[nameof(SchoolOutDto.Name).Camelize()] = school.Name;
            dictSchool[nameof(SchoolOutDto.YearOfEstablishment).Camelize()] = school.YearOfEstablishment;

            return shapedSchool;
        }

        private ExpandoObject ExpectedShapedByRelatedDataLevel1(SchoolOutDto school)
        {
            var shapedSchool = new ExpandoObject();
            var dictSchool = (IDictionary<string, object>)shapedSchool;

            dictSchool[nameof(SchoolOutDto.Id).Camelize()] = school.Id;
            dictSchool[nameof(SchoolOutDto.Name).Camelize()] = school.Name;
            dictSchool[nameof(SchoolOutDto.YearOfEstablishment).Camelize()] = school.YearOfEstablishment;
            dictSchool[nameof(SchoolOutDto.Lessons).Camelize()] = CreateDetail(school.Lessons, nameof(LessonOutDto.Id), nameof(LessonOutDto.Name), nameof(LessonOutDto.WeekHours));
            dictSchool[nameof(SchoolOutDto.Students).Camelize()] = CreateDetail(school.Students, nameof(StudentOutDto.Id), nameof(StudentOutDto.FirstName), nameof(StudentOutDto.LastName), nameof(StudentOutDto.FullName), nameof(StudentOutDto.DateOfBirth));

            return shapedSchool;
        }

        private ExpandoObject ExpectedShapedAll(SchoolOutDto school)
        {
            var obj = new ExpandoObject();
            var dictSchool = (IDictionary<string, object>)obj;

            dictSchool[nameof(SchoolOutDto.Id).Camelize()] = school.Id;
            dictSchool[nameof(SchoolOutDto.Name).Camelize()] = school.Name;
            dictSchool[nameof(SchoolOutDto.YearOfEstablishment).Camelize()] = school.YearOfEstablishment;

            var shapedLessons = new List<ExpandoObject>();

            foreach (var lesson in school.Lessons)
            {
                var shapedLesson = CreateObject(lesson, nameof(LessonOutDto.Id), nameof(LessonOutDto.Name), nameof(LessonOutDto.WeekHours));
                var dictLesson = (IDictionary<string, object>)shapedLesson;

                dictLesson[nameof(LessonOutDto.Books).Camelize()] = CreateDetail(lesson.Books, nameof(BookOutDto.Id), nameof(BookOutDto.Name));

                shapedLessons.Add(shapedLesson);
            }

            dictSchool[nameof(SchoolOutDto.Lessons).Camelize()] = shapedLessons;

            var shapedStudents = new List<ExpandoObject>();

            foreach (var student in school.Students)
            {
                var shapedStudent = CreateObject(student, nameof(StudentOutDto.Id), nameof(StudentOutDto.FirstName), nameof(StudentOutDto.LastName), nameof(StudentOutDto.FullName), nameof(StudentOutDto.DateOfBirth));
                var dictStudent = (IDictionary<string, object>)shapedStudent;

                var shapedContactInfo = CreateObject(student.ContactInfo, nameof(ContactInfoOutDto.Id), nameof(ContactInfoOutDto.Email), nameof(ContactInfoOutDto.Address));

                if (student.ContactInfo != null)
                {
                    var dictContactInfo = (IDictionary<string, object>)shapedContactInfo;
                    foreach (var phone in student.ContactInfo.Phones)
                    {
                        dictContactInfo[nameof(ContactInfoOutDto.Phones).Camelize()] = CreateDetail(student.ContactInfo.Phones, nameof(PhoneOutDto.Id), nameof(PhoneOutDto.Number));
                    }
                }

                dictStudent[nameof(StudentOutDto.ContactInfo).Camelize()] = shapedContactInfo;

                shapedStudents.Add(shapedStudent);
            }

            dictSchool[nameof(SchoolOutDto.Students).Camelize()] = shapedStudents;

            return obj;
        }
    }
}
