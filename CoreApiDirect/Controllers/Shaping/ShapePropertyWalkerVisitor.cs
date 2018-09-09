using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using CoreApiDirect.Base;
using CoreApiDirect.Infrastructure;

namespace CoreApiDirect.Controllers.Shaping
{
    internal class ShapePropertyWalkerVisitor : PropertyWalkerVisitor<ExpandoObject, ShapeWalkInfo>, IShapePropertyWalkerVisitor
    {
        private readonly IFieldNameResolver _fieldNameResolver;
        private readonly ExpandoObject _object = new ExpandoObject();

        public ShapePropertyWalkerVisitor(
            IServiceProvider serviceProvider,
            IFieldNameResolver fieldNameResolver)
            : base(serviceProvider)
        {
            _fieldNameResolver = fieldNameResolver;
        }

        public override ExpandoObject Output(ShapeWalkInfo walkInfo)
        {
            return _object;
        }

        public override void Visit(PropertyInfo property, ShapeWalkInfo walkInfo)
        {
            var value = walkInfo.Object.GetPropertyValue(property.Name);

            if (value != null && property.PropertyType.IsDetailOfRawGeneric(walkInfo.GenericDefinition))
            {
                if (property.PropertyType.IsListOfRawGeneric(walkInfo.GenericDefinition))
                {
                    value = (value as IEnumerable<object>).Select(p => NextWalk(p, walkInfo));
                }
                else
                {
                    value = NextWalk(value, walkInfo);
                }
            }

            ((IDictionary<string, object>)_object)[_fieldNameResolver.GetFieldName(property.Name)] = value;
        }

        private ExpandoObject NextWalk(object obj, ShapeWalkInfo walkInfo)
        {
            return NextWalk<ExpandoObject, ShapeWalkInfo>(GeNextWalkInfo(obj, walkInfo), typeof(IShapePropertyWalker), typeof(IShapePropertyWalkerVisitor));
        }

        private ShapeWalkInfo GeNextWalkInfo(object obj, ShapeWalkInfo walkInfo)
        {
            var type = obj.GetType();

            return new ShapeWalkInfo
            {
                Object = obj,
                Type = type,
                GenericDefinition = type.BaseGenericType().GetGenericTypeDefinition(),
                WalkedTypes = walkInfo.WalkedTypes.ToList(),
                Fields = walkInfo.Fields,
                RelatedDataLevel = walkInfo.RelatedDataLevel
            };
        }
    }
}
