using System;
using System.Linq;
using CoreApiDirect.Base;
using CoreApiDirect.Demo.Entities.App;
using CoreApiDirect.Tests.DataContext;
using Xunit;

namespace CoreApiDirect.Tests.Base
{
    public class ObjectExtensionsTests
    {
        private const string INVALID_PROPERTY = "InvalidProperty";

        private readonly School _school = AppDbContextTests.GetContextWithData().Schools.FirstOrDefault();

        [Fact]
        public void GetPropertyValue_PropertyNameIsNull_ExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() => _school.GetPropertyValue(null));
        }

        [Fact]
        public void GetPropertyValue_PropertyNameIsInvalid_ExceptionThrown()
        {
            Assert.Throws<ArgumentException>(() => _school.GetPropertyValue(INVALID_PROPERTY));
        }

        [Fact]
        public void GetPropertyValue_PropertyNameIsValid_PropertyValue()
        {
            Assert.Equal(_school.GetPropertyValue(nameof(School.Name)), _school.Name);
        }

        [Fact]
        public void SetPropertyValue_PropertyNameIsNull_ExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() => _school.SetPropertyValue(null, null));
        }

        [Fact]
        public void SetPropertyValue_PropertyNameIsInvalid_ExceptionThrown()
        {
            Assert.Throws<ArgumentException>(() => _school.SetPropertyValue(INVALID_PROPERTY, null));
        }

        [Fact]
        public void SetPropertyValue_PropertyNameIsValid_ValueSet()
        {
            string value = "School Name";
            _school.SetPropertyValue(nameof(School.Name), value);
            Assert.Equal(value, _school.Name);
        }
    }
}
