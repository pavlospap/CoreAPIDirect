using System;
using CoreApiDirect.Controllers;
using CoreApiDirect.Demo.Entities.App;
using CoreApiDirect.Repositories;
using CoreApiDirect.Tests.DataContext;
using CoreApiDirect.Tests.Options;
using CoreApiDirect.Url;
using Xunit;

namespace CoreApiDirect.Tests.Controllers
{
    public class PagedListTests
    {
        private const int TOTAL_PAGES = 8;

        private readonly IRepository<Student, int> _repository;
        private readonly int _middlePage;

        public PagedListTests()
        {
            _repository = new Repository<Student, int, AppDbContextTests>(AppDbContextTests.GetContextWithData());
            _middlePage = new Random().Next(2, TOTAL_PAGES - 1);
        }

        [Fact]
        public void TotalPages_GetValue_TotalPages()
        {
            Assert.Equal(TOTAL_PAGES, GetPagedList(1).TotalPages);
        }

        [Fact]
        public void HasPrevious_FirstPage_False()
        {
            Assert.False(GetPagedList(1).HasPrevious);
        }

        [Fact]
        public void HasPrevious_MiddlePage_True()
        {
            Assert.True(GetPagedList(_middlePage).HasPrevious);
        }

        [Fact]
        public void HasNext_MiddlePage_True()
        {
            Assert.True(GetPagedList(_middlePage).HasNext);
        }

        [Fact]
        public void HasNext_LastPage_False()
        {
            Assert.False(GetPagedList(TOTAL_PAGES).HasNext);
        }

        private PagedList<Student> GetPagedList(int pageNumber)
        {
            return PagedList<Student>.CreateAsync(_repository.Query,
                new QueryString(new CoreOptionsTests().Value) { PageNumber = pageNumber }).Result;
        }
    }
}
