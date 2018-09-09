using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace CoreApiDirect.Base
{
    internal class PropertyProvider : IPropertyProvider
    {
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private readonly Dictionary<Type, PropertyInfo[]> _properties = new Dictionary<Type, PropertyInfo[]>();

        public PropertyInfo[] GetProperties(Type type)
        {
            type.ValidateNull(nameof(type));

            _lock.EnterWriteLock();

            try
            {
                if (!_properties.ContainsKey(type))
                {
                    _properties[type] = type.GetProperties();
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }

            return _properties[type];
        }
    }
}
