using System;
using CoreApiDirect.Base;
using Xunit;

namespace CoreApiDirect.Tests.Base
{
    public class ExceptionExtensionsTests
    {
        private const string EXCEPTION_MESSAGE = "Exception message";

        [Fact]
        public void MostInnerMessage_InnerExceptionIsNull_ExceptionMessage()
        {
            var ex = new Exception(EXCEPTION_MESSAGE);
            Assert.Equal(EXCEPTION_MESSAGE, ex.MostInnerMessage());
        }

        [Fact]
        public void MostInnerMessage_InnerExceptionIsNotNull_InnerExceptionMessage()
        {
            string innerExceptionMessage = "Inner exception message";
            var ex = new Exception(EXCEPTION_MESSAGE, new Exception(innerExceptionMessage));
            Assert.Equal(innerExceptionMessage, ex.MostInnerMessage());
        }
    }
}
