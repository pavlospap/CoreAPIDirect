using System;
using System.Collections.Generic;
using CoreApiDirect.Base;
using Xunit;

namespace CoreApiDirect.Tests.Base
{
    public class IDictionaryExtentionsTests
    {
        private readonly IDictionary<string, string> _dictionary = new Dictionary<string, string> { { "key1", "value1" } };

        [Fact]
        public void GetValueIgnoreCase_KeyDoesNotExist_Null()
        {
            Assert.Throws<ArgumentException>(() => _dictionary.GetValueIgnoreCase("key2"));
        }

        [Fact]
        public void GetValueIgnoreCase_KeyExists_Value()
        {
            Assert.Equal("value1", _dictionary.GetValueIgnoreCase("KEY1"));
        }
    }
}
