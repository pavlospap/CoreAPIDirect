using System.Collections.Generic;
using System.Linq;
using CoreApiDirect.Base;
using Xunit;

namespace CoreApiDirect.Tests.Base
{
    public class ListProviderTests
    {
        private readonly ListProvider _listProvider = new ListProvider();

        [Fact]
        public void GetTypedList_StringValuesToIntegerList_TypedList()
        {
            var typedList = _listProvider.GetTypedList(new string[] { "1", "2", "3" }, typeof(int), typeof(List<>)) as List<int>;
            Assert.True(Enumerable.SequenceEqual(typedList, new List<int> { 1, 2, 3 }));
        }

        [Fact]
        public void GetTypedList_StringValuesToStringList_TypedList()
        {
            var typedList = _listProvider.GetTypedList(new string[] {
                "67acf233-8baa-4359-b308-dcd8e1e36361",
                "6c74604e-5748-4e4b-9434-67dabe0afb5f",
                "24278af6-e710-4fd4-9bec-8bbcf343fc9d" }, typeof(string), typeof(List<>)) as List<string>;
            Assert.True(Enumerable.SequenceEqual(typedList, new List<string> {
                "67acf233-8baa-4359-b308-dcd8e1e36361",
                "6c74604e-5748-4e4b-9434-67dabe0afb5f",
                "24278af6-e710-4fd4-9bec-8bbcf343fc9d" }));
        }
    }
}
