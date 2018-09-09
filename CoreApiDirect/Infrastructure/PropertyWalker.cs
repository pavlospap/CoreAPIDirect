using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CoreApiDirect.Base;

namespace CoreApiDirect.Infrastructure
{
    internal abstract class PropertyWalker<TResult, TWalkInfo> : IPropertyWalker<TResult, TWalkInfo>
        where TWalkInfo : WalkInfo
    {
        private readonly IPropertyProvider _propertyProvider;

        public PropertyWalker(IPropertyProvider propertyProvider)
        {
            _propertyProvider = propertyProvider;
        }

        public virtual TResult Accept(IPropertyWalkerVisitor<TResult, TWalkInfo> visitor, TWalkInfo walkInfo)
        {
            walkInfo.WalkedTypes.Add(walkInfo.Type);

            var fieldFirstParts = GetFirstPartsFromFields(walkInfo.Fields);
            var properties = GetProperties(walkInfo, fieldFirstParts);

            if (!walkInfo.Fields.Contains("*"))
            {
                walkInfo.Fields = walkInfo.Fields.Except(fieldFirstParts);
            }

            walkInfo.RelatedDataLevel--;

            foreach (var property in properties)
            {
                WalkIntoProperty(visitor, property, walkInfo);
            }

            return visitor.Output(walkInfo);
        }

        private IEnumerable<string> GetFirstPartsFromFields(IEnumerable<string> fields)
        {
            return fields.Select(p => p.Split('.')[0]).Distinct();
        }

        private IEnumerable<PropertyInfo> GetProperties(TWalkInfo walkInfo, IEnumerable<string> fields)
        {
            var properties = _propertyProvider.GetProperties(walkInfo.Type);

            if ((!fields.Any() && walkInfo.RelatedDataLevel > 0) || fields.Contains("*"))
            {
                return properties.Where(p => !p.PropertyType.IsDetailOfRawGeneric(walkInfo.GenericDefinition) ||
                                             IsDetailAndNotExistInWalkedTypes(p.PropertyType, walkInfo));
            }
            else if (!fields.Any() && walkInfo.RelatedDataLevel <= 0)
            {
                return properties.Where(p => !p.PropertyType.IsDetailOfRawGeneric(walkInfo.GenericDefinition));
            }
            else
            {
                return properties.Where(p => fields.Contains(p.Name, StringComparer.OrdinalIgnoreCase) &&
                                             (!p.PropertyType.IsDetailOfRawGeneric(walkInfo.GenericDefinition) ||
                                              IsDetailAndNotExistInWalkedTypes(p.PropertyType, walkInfo)));
            }
        }

        private bool IsDetailAndNotExistInWalkedTypes(Type type, TWalkInfo walkInfo)
        {
            return (type.IsListOfRawGeneric(walkInfo.GenericDefinition) && !walkInfo.WalkedTypes.Contains(type.BaseGenericType().GenericTypeArguments[0])) ||
                (type.IsSubclassOfRawGeneric(walkInfo.GenericDefinition) && !walkInfo.WalkedTypes.Contains(type));
        }

        private void WalkIntoProperty(IPropertyWalkerVisitor<TResult, TWalkInfo> visitor, PropertyInfo property, TWalkInfo walkInfo)
        {
            var fieldsWithPropertyPrefix = new List<string>();

            var propertyWalkInfoFields = walkInfo.Fields.Contains("*") ?
                new List<string> { "*" } :
                RemovePropertyPrefixFromFields(fieldsWithPropertyPrefix = GetFieldsWithPropertyPrefix(walkInfo.Fields, property.Name).ToList(), property.Name);

            visitor.Visit(property, BuildWalkInfoForProperty(walkInfo, propertyWalkInfoFields));

            walkInfo.Fields = walkInfo.Fields.Except(fieldsWithPropertyPrefix, StringComparer.OrdinalIgnoreCase);
        }

        private IEnumerable<string> GetFieldsWithPropertyPrefix(IEnumerable<string> fields, string propertyName)
        {
            return fields.Where(p => p.StartsWith(propertyName + ".", StringComparison.OrdinalIgnoreCase));
        }

        private List<string> RemovePropertyPrefixFromFields(IEnumerable<string> fields, string propertyName)
        {
            return fields.Select(p => p.Substring(propertyName.Length + 1)).ToList();
        }

        private TWalkInfo BuildWalkInfoForProperty(TWalkInfo walkInfo, IEnumerable<string> fields)
        {
            var newWalkInfo = Activator.CreateInstance(GetWalkInfoTypeForProperty(walkInfo)) as TWalkInfo;

            foreach (var property in walkInfo.GetType().GetProperties())
            {
                newWalkInfo.SetPropertyValue(property.Name, walkInfo.GetPropertyValue(property.Name));
            }

            newWalkInfo.Fields = fields;

            return newWalkInfo;
        }

        protected virtual Type GetWalkInfoTypeForProperty(TWalkInfo walkInfo)
        {
            return typeof(TWalkInfo);
        }
    }
}
