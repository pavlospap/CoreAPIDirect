using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreApiDirect.Base;
using CoreApiDirect.Demo.Entities.App;
using CoreApiDirect.Options;
using CoreApiDirect.Query;
using CoreApiDirect.Query.Detail;
using CoreApiDirect.Query.Filter;
using CoreApiDirect.Query.Operators;
using CoreApiDirect.Query.Parameters;
using CoreApiDirect.Repositories;
using CoreApiDirect.Tests.DataContext;
using CoreApiDirect.Tests.Query.Helpers;
using CoreApiDirect.Url;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CoreApiDirect.Tests.Query
{
    public class QueryBuilderTests
    {
        [Fact]
        public void Build_SearchKeyOnNonStringProperty_ExceptionThrown()
        {
            var invalidEntitiesRepository = new Repository<InvalidEntity, int, AppDbContextTests>(AppDbContextTests.GetContextWithData());
            var builder = GetQueryBuilder<InvalidEntity>(new ServiceCollection());

            Assert.Throws<InvalidOperationException>(() => builder.Build(invalidEntitiesRepository.Query, new QueryString(new CoreOptions())
            {
                QueryParams = new QueryParams
                {
                    Search = "anything"
                }
            }));
        }

        [Fact]
        public async Task Build_ValidData_EqualToExpected()
        {
            var schoolsRepository = new Repository<School, int, AppDbContextTests>(AppDbContextTests.GetContextWithData());

            var expectedDataQuery = schoolsRepository.Query
                .Where(school => school.Name.Contains("of") == true) // Search
                .Where(school => school.YearOfEstablishment > 1739 && // Greater
                                 school.YearOfEstablishment < 1852 || // Less
                                 school.Lessons.Any(lesson => lesson.Books.Any(book => book.Name == "The art of computer programming"))) // Equal
                .OrderByDescending(school => school.YearOfEstablishment)
                .Select(school => new School
                {
                    Name = school.Name,
                    YearOfEstablishment = school.YearOfEstablishment,
                    Lessons = school.Lessons
                        .Where(lesson => new List<object> { 2, 3, 4, 5 }.Contains(lesson.WeekHours) == true) // In
                        .OrderBy(lesson => lesson.Name)
                        .Select(lesson => new Lesson
                        {
                            Id = lesson.Id,
                            Name = lesson.Name,
                            Books = lesson.Books
                                .Where(book => book.Name.Contains("The") == true && // Like
                                               book.Name.Contains("math") == false) // NotLike
                                .OrderByDescending(book => book.Name)
                                .Select(book => new Book
                                {
                                    Id = book.Id,
                                    Name = book.Name,
                                    LessonId = book.LessonId
                                })
                                .ToList()
                        })
                        .ToList(),
                    Students = school.Students
                        .Where(student => student.DateOfBirth >= new DateTime(1996, 1, 1) && // GreaterOrEqual
                                          student.DateOfBirth <= new DateTime(2000, 12, 31) && // LessOrEqual
                                          student.ContactInfo.Email != null || // NotNull
                                          student.ContactInfo.Address == null) // Null
                        .OrderBy(student => student.FirstName)
                        .ThenByDescending(student => student.LastName)
                        .Select(student => new Student
                        {
                            FirstName = student.FirstName,
                            LastName = student.LastName,
                            ContactInfo = student.ContactInfo != default(ContactInfo) ? new ContactInfo
                            {
                                Email = student.ContactInfo.Email,
                                Phones = student.ContactInfo.Phones
                                    .Where(phone => phone.Number != "608-555-0122" && // NotEqual
                                                    new List<object> { "405-555-0176", "302-555-0147" }.Contains(phone.Number) == false) // NotIn
                                    .OrderBy(phone => phone.Number)
                                    .Select(phone => new Phone
                                    {
                                        Number = phone.Number
                                    })
                                    .ToList()
                            } : default
                        })
                        .ToList()
                });

            var expectedData = await expectedDataQuery.ToListAsync();

            var builder = GetQueryBuilder<School>(GetServices());

            var builtDataQuery = builder.Build(schoolsRepository.Query, new QueryString(new CoreOptions())
            {
                QueryParams = new QueryParams
                {
                    Fields = new List<string>
                    {
                        "name",
                        "yearofestablishment",
                        "lessons.id",
                        "lessons.name",
                        "lessons.books.*",
                        "students.firstname",
                        "students.lastname",
                        "students.contactinfo.email",
                        "students.contactinfo.phones.number"
                    },
                    Search = "of",
                    Filter = new List<QueryLogicalFilter>
                    {
                        new QueryLogicalFilter
                        {
                            Operator = LogicalOperator.And,
                            Filter = new QueryComparisonFilter {
                                Field = "yearofestablishment",
                                Operator = ComparisonOperator.Greater,
                                Values = new List<string> { "1739" }
                            }
                        },
                        new QueryLogicalFilter
                        {
                            Operator = LogicalOperator.And,
                            Filter = new QueryComparisonFilter {
                                Field = "yearofestablishment",
                                Operator = ComparisonOperator.Less,
                                Values = new List<string> { "1852" }
                            }
                        },
                        new QueryLogicalFilter
                        {
                            Operator = LogicalOperator.Or,
                            Filter = new QueryComparisonFilter {
                                Field = "lessons.books.name",
                                Operator = ComparisonOperator.Equal,
                                Values = new List<string> { "The art of computer programming" }
                            }
                        },
                        new QueryLogicalFilter
                        {
                            Operator = LogicalOperator.And,
                            Filter = new QueryComparisonFilter {
                                Field = "lessons!.weekhours",
                                Operator = ComparisonOperator.In,
                                Values = new List<string> { "2", "3", "4", "5" }
                            }
                        },
                        new QueryLogicalFilter
                        {
                            Operator = LogicalOperator.And,
                            Filter = new QueryComparisonFilter {
                                Field = "lessons.books!.name",
                                Operator = ComparisonOperator.Like,
                                Values = new List<string> { "The" }
                            }
                        },
                        new QueryLogicalFilter
                        {
                            Operator = LogicalOperator.And,
                            Filter = new QueryComparisonFilter {
                                Field = "lessons.books!.name",
                                Operator = ComparisonOperator.NotLike,
                                Values = new List<string> { "math" }
                            }
                        },
                        new QueryLogicalFilter
                        {
                            Operator = LogicalOperator.And,
                            Filter = new QueryComparisonFilter {
                                Field = "students!.dateofbirth",
                                Operator = ComparisonOperator.GreaterOrEqual,
                                Values = new List<string> {  new DateTime(1996, 1, 1).ToLongDateString() }
                            }
                        },
                        new QueryLogicalFilter
                        {
                            Operator = LogicalOperator.And,
                            Filter = new QueryComparisonFilter {
                                Field = "students!.dateofbirth",
                                Operator = ComparisonOperator.LessOrEqual,
                                Values = new List<string> {  new DateTime(2000, 12, 31).ToLongDateString() }
                            }
                        },
                        new QueryLogicalFilter
                        {
                            Operator = LogicalOperator.And,
                            Filter = new QueryComparisonFilter {
                                Field = "students!.contactinfo.email",
                                Operator = ComparisonOperator.NotNull
                            }
                        },
                        new QueryLogicalFilter
                        {
                            Operator = LogicalOperator.Or,
                            Filter = new QueryComparisonFilter {
                                Field = "students!.contactinfo.address",
                                Operator = ComparisonOperator.Null
                            }
                        },
                        new QueryLogicalFilter
                        {
                            Operator = LogicalOperator.And,
                            Filter = new QueryComparisonFilter {
                                Field = "students.contactinfo.phones!.number",
                                Operator = ComparisonOperator.NotEqual,
                                Values = new List<string> { "608-555-0122" }
                            }
                        },
                        new QueryLogicalFilter
                        {
                            Operator = LogicalOperator.And,
                            Filter = new QueryComparisonFilter {
                                Field = "students.contactinfo.phones!.number",
                                Operator = ComparisonOperator.NotIn,
                                Values = new List<string> { "405-555-0176", "302-555-0147" }
                            }
                        }
                    },
                    Sort = new List<QuerySort>
                    {
                        new QuerySort
                        {
                            Field = "yearofestablishment",
                            Direction = SortDirection.Descending
                        },
                        new QuerySort
                        {
                            Field = "lessons.name",
                            Direction = SortDirection.Ascending
                        },
                        new QuerySort
                        {
                            Field = "lessons.books.name",
                            Direction = SortDirection.Descending
                        },
                        new QuerySort
                        {
                            Field = "students.firstname",
                            Direction = SortDirection.Ascending
                        },
                        new QuerySort
                        {
                            Field = "students.lastname",
                            Direction = SortDirection.Descending
                        },
                        new QuerySort
                        {
                            Field = "students.contactinfo.phones.number",
                            Direction = SortDirection.Ascending
                        }
                    }
                }
            });

            var builtData = await builtDataQuery.ToListAsync();

            Assert.Equal(expectedDataQuery.ToJson(), builtDataQuery.ToJson());
        }

        private QueryBuilder<TEntity> GetQueryBuilder<TEntity>(IServiceCollection services)
        {
            var walker = new QueryPropertyWalker<TEntity>(new PropertyProvider());
            var visitor = new QueryPropertyWalkerVisitor<TEntity>(
                services.BuildServiceProvider(),
                new PropertyProvider(),
                new MethodProvider());
            return new QueryBuilder<TEntity>(walker, visitor);
        }

        private IServiceCollection GetServices()
        {
            var services = new ServiceCollection();
            services.AddTransient<IOneToManyQueryDetailPropertyWalker<Lesson>, OneToManyQueryDetailPropertyWalker<Lesson>>();
            services.AddTransient<IOneToManyQueryDetailPropertyWalkerVisitor<Lesson>, OneToManyQueryDetailPropertyWalkerVisitor<Lesson>>();
            services.AddTransient<IOneToManyQueryDetailPropertyWalker<Book>, OneToManyQueryDetailPropertyWalker<Book>>();
            services.AddTransient<IOneToManyQueryDetailPropertyWalkerVisitor<Book>, OneToManyQueryDetailPropertyWalkerVisitor<Book>>();
            services.AddTransient<IOneToManyQueryDetailPropertyWalker<Student>, OneToManyQueryDetailPropertyWalker<Student>>();
            services.AddTransient<IOneToManyQueryDetailPropertyWalkerVisitor<Student>, OneToManyQueryDetailPropertyWalkerVisitor<Student>>();
            services.AddTransient<IOneToOneQueryDetailPropertyWalker<ContactInfo>, OneToOneQueryDetailPropertyWalker<ContactInfo>>();
            services.AddTransient<IOneToOneQueryDetailPropertyWalkerVisitor<ContactInfo>, OneToOneQueryDetailPropertyWalkerVisitor<ContactInfo>>();
            services.AddTransient<IOneToManyQueryDetailPropertyWalker<Phone>, OneToManyQueryDetailPropertyWalker<Phone>>();
            services.AddTransient<IOneToManyQueryDetailPropertyWalkerVisitor<Phone>, OneToManyQueryDetailPropertyWalkerVisitor<Phone>>();
            services.AddTransient<IQueryFilterPropertyWalker, QueryFilterPropertyWalker>();
            services.AddTransient<IQueryFilterPropertyWalkerVisitor, QueryFilterPropertyWalkerVisitor>();
            services.AddSingleton<IPropertyProvider, PropertyProvider>();
            services.AddSingleton<IMethodProvider, MethodProvider>();
            services.AddSingleton<IListProvider, ListProvider>();

            return services;
        }
    }
}
