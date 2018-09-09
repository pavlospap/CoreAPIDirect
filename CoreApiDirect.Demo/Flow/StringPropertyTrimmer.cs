using System.Linq;
using CoreApiDirect.Base;

namespace CoreApiDirect.Demo.Flow
{
    public class StringPropertyTrimmer
    {
        public static void Trim<TEntity>(TEntity entity)
        {
            var stringProperties = entity.GetType().GetProperties().Where(p => p.PropertyType == typeof(string));
            foreach (var property in stringProperties)
            {
                if (entity.GetPropertyValue(property.Name) is string value)
                {
                    entity.SetPropertyValue(property.Name, value.Trim());
                }
            }
        }
    }
}
