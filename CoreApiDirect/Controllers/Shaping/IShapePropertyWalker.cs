using System.Dynamic;
using CoreApiDirect.Infrastructure;

namespace CoreApiDirect.Controllers.Shaping
{
    internal interface IShapePropertyWalker : IPropertyWalker<ExpandoObject, ShapeWalkInfo>
    {
    }
}
