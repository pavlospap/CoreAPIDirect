using System.Dynamic;
using CoreApiDirect.Url;

namespace CoreApiDirect.Controllers.Shaping
{
    internal interface IShaper
    {
        ExpandoObject Shape(object obj, QueryString queryString);
    }
}
