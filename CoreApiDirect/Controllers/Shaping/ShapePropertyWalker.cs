using System.Dynamic;
using CoreApiDirect.Base;
using CoreApiDirect.Infrastructure;

namespace CoreApiDirect.Controllers.Shaping
{
    internal class ShapePropertyWalker : PropertyWalker<ExpandoObject, ShapeWalkInfo>, IShapePropertyWalker
    {
        public ShapePropertyWalker(IPropertyProvider propertyProvider)
            : base(propertyProvider)
        {
        }
    }
}
