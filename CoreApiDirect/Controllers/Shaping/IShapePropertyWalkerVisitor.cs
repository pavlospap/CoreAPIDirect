using System.Dynamic;
using CoreApiDirect.Infrastructure;

namespace CoreApiDirect.Controllers.Shaping
{
    internal interface IShapePropertyWalkerVisitor : IPropertyWalkerVisitor<ExpandoObject, ShapeWalkInfo>
    {
    }
}
