using System.Dynamic;
using CoreApiDirect.Base;
using CoreApiDirect.Url;

namespace CoreApiDirect.Controllers.Shaping
{
    internal class Shaper : IShaper
    {
        private readonly IShapePropertyWalker _walker;
        private readonly IShapePropertyWalkerVisitor _visitor;

        public Shaper(
            IShapePropertyWalker walker,
            IShapePropertyWalkerVisitor visitor)
        {
            _walker = walker;
            _visitor = visitor;
        }

        public ExpandoObject Shape(object obj, QueryString queryString)
        {
            var type = obj.GetType();

            return _walker.Accept(_visitor, new ShapeWalkInfo
            {
                Object = obj,
                Type = type,
                GenericDefinition = type.BaseGenericType().GetGenericTypeDefinition(),
                Fields = queryString.QueryParams.Fields,
                RelatedDataLevel = queryString.RelatedDataLevel
            });
        }
    }
}
