using System;
using System.Collections.Generic;
using CoreApiDirect.Base;
using CoreApiDirect.Demo.Entities.App;
using CoreApiDirect.Demo.Repositories;
using CoreApiDirect.Entities;
using CoreApiDirect.Repositories;
using CoreApiDirect.Tests.Base.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CoreApiDirect.Tests.Base
{
    public class TypeExtensionsTests
    {
        [Fact]
        public void IsSubclassOfRawGeneric_GenericDefinitionIsNull_ExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() => typeof(School).IsSubclassOfRawGeneric(null));
        }

        [Fact]
        public void IsSubclassOfRawGeneric_GenericDefinitionIsNotGeneric_ExceptionThrown()
        {
            Assert.Throws<ArgumentException>(() => typeof(School).IsSubclassOfRawGeneric(typeof(NonGeneric)));
        }

        [Fact]
        public void IsSubclassOfRawGeneric_GenericDefinitionIsInterface_ExceptionThrown()
        {
            Assert.Throws<ArgumentException>(() => typeof(School).IsSubclassOfRawGeneric(typeof(IGeneric<>)));
        }

        [Fact]
        public void IsSubclassOfRawGeneric_TypeIsNotSubclassOfGenericDefinition_False()
        {
            Assert.False(typeof(School).IsSubclassOfRawGeneric(typeof(Generic<>)));
        }

        [Fact]
        public void IsSubclassOfRawGeneric_TypeIsSubclassOfGenericDefinition_True()
        {
            Assert.True(typeof(School).IsSubclassOfRawGeneric(typeof(Entity<>)));
        }

        [Fact]
        public void ImplementsRawGenericInterface_GenericDefinitionIsNull_ExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() => typeof(SchoolsRepository).ImplementsRawGenericInterface(null));
        }

        [Fact]
        public void ImplementsRawGenericInterface_GenericDefinitionIsNotGeneric_ExceptionThrown()
        {
            Assert.Throws<ArgumentException>(() => typeof(SchoolsRepository).ImplementsRawGenericInterface(typeof(INonGeneric)));
        }

        [Fact]
        public void ImplementsRawGenericInterface_GenericDefinitionIsNotInterface_ExceptionThrown()
        {
            Assert.Throws<ArgumentException>(() => typeof(SchoolsRepository).ImplementsRawGenericInterface(typeof(Generic<>)));
        }

        [Fact]
        public void ImplementsRawGenericInterface_TypeDoesNotImplementGenericDefinition_False()
        {
            Assert.False(typeof(SchoolsRepository).ImplementsRawGenericInterface(typeof(IGeneric<>)));
        }

        [Fact]
        public void ImplementsRawGenericInterface_TypeImplementsGenericDefinition_True()
        {
            Assert.True(typeof(SchoolsRepository).ImplementsRawGenericInterface(typeof(IRepository<,>)));
        }

        [Fact]
        public void IsListOfRawGeneric_GenericDefinitionIsNull_ExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() => typeof(HashSet<School>).IsListOfRawGeneric(null));
        }

        [Fact]
        public void IsListOfRawGeneric_GenericDefinitionIsNotGeneric_ExceptionThrown()
        {
            Assert.Throws<ArgumentException>(() => typeof(HashSet<School>).IsListOfRawGeneric(typeof(NonGeneric)));
        }

        [Fact]
        public void IsListOfRawGeneric_GenericDefinitionIsInterface_ExceptionThrown()
        {
            Assert.Throws<ArgumentException>(() => typeof(HashSet<School>).IsListOfRawGeneric(typeof(IGeneric<>)));
        }

        [Fact]
        public void IsListOfRawGeneric_TypeIsNotList_False()
        {
            Assert.False(typeof(School).IsListOfRawGeneric(typeof(Entity<>)));
        }

        [Fact]
        public void IsListOfRawGeneric_TypeIsNotListOfGenericDefinition_False()
        {
            Assert.False(typeof(HashSet<School>).IsListOfRawGeneric(typeof(Generic<>)));
        }

        [Fact]
        public void IsListOfRawGeneric_TypeIsListOfGenericDefinition_True()
        {
            Assert.True(typeof(HashSet<School>).IsListOfRawGeneric(typeof(Entity<>)));
        }

        [Fact]
        public void IsDetailOfRawGeneric_GenericDefinitionIsNull_ExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() => typeof(HashSet<School>).IsDetailOfRawGeneric(null));
        }

        [Fact]
        public void IsDetailOfRawGeneric_GenericDefinitionIsNotGeneric_ExceptionThrown()
        {
            Assert.Throws<ArgumentException>(() => typeof(HashSet<School>).IsDetailOfRawGeneric(typeof(NonGeneric)));
        }

        [Fact]
        public void IsDetailOfRawGeneric_GenericDefinitionIsInterface_ExceptionThrown()
        {
            Assert.Throws<ArgumentException>(() => typeof(HashSet<School>).IsDetailOfRawGeneric(typeof(IGeneric<>)));
        }

        [Theory]
        [InlineData(typeof(HashSet<School>))]
        [InlineData(typeof(ContactInfo))]
        public void IsDetailOfRawGeneric_TypeIsNotDetailOfGenericDefinition_False(Type type)
        {
            Assert.False(type.IsDetailOfRawGeneric(typeof(Generic<>)));
        }

        [Theory]
        [InlineData(typeof(HashSet<School>))]
        [InlineData(typeof(ContactInfo))]
        public void IsDetailOfRawGeneric_TypeIsDetailOfGenericDefinition_True(Type type)
        {
            Assert.True(type.IsDetailOfRawGeneric(typeof(Entity<>)));
        }

        [Fact]
        public void IsListOfType_ElementTypeIsNull_ExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() => typeof(DbSet<School>).IsListOfType(null));
        }

        [Fact]
        public void IsListOfType_TypeIsNotList_False()
        {
            Assert.False(typeof(School).IsListOfType(typeof(School)));
        }

        [Fact]
        public void IsListOfType_TypeIsNotListOfElementType_False()
        {
            Assert.False(typeof(DbSet<School>).IsListOfType(typeof(NonGeneric)));
        }

        [Fact]
        public void IsListOfType_TypeIsListOfElementType_True()
        {
            Assert.True(typeof(DbSet<School>).IsListOfType(typeof(School)));
        }

        [Fact]
        public void BaseGenericType_TypeHasNotBaseGenericType_Null()
        {
            Assert.Null(typeof(NonGeneric).BaseGenericType());
        }

        [Fact]
        public void BaseGenericType_TypeHasBaseGenericType_BaseGenericType()
        {
            Assert.Equal(typeof(Entity<int>), typeof(School).BaseGenericType());
        }

        [Fact]
        public void GetPropertyIgnoreCase_PropertyNameIsNull_ExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() => typeof(School).GetPropertyIgnoreCase(null));
        }

        [Fact]
        public void GetPropertyIgnoreCase_PropertyNameIsInvalid_Null()
        {
            Assert.Null(typeof(School).GetPropertyIgnoreCase("InvalidProperty"));
        }

        [Fact]
        public void GetPropertyIgnoreCase_PropertyNameIsValid_Property()
        {
            Assert.NotNull(typeof(School).GetPropertyIgnoreCase(nameof(School.Name).ToLower()));
        }
    }
}
