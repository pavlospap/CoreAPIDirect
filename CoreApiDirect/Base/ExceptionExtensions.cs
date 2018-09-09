using System;

namespace CoreApiDirect.Base
{
    internal static class ExceptionExtensions
    {
        public static string MostInnerMessage(this Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            return ex.Message;
        }
    }
}
