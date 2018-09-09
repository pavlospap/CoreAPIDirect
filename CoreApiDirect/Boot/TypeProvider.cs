using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace CoreApiDirect.Boot
{
    internal class TypeProvider : ITypeProvider
    {
        public static TypeProvider Instance
        {
            get
            {
                return _instance ?? (_instance = new TypeProvider());
            }
        }

        public List<Type> Types { get; } = new List<Type>();

        private static TypeProvider _instance;

        private TypeProvider()
        {
            GetAllTypesFromCurrentLocation();
        }

        private void GetAllTypesFromCurrentLocation()
        {
            string location = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            foreach (string file in Directory.GetFiles(location, "*.dll", SearchOption.AllDirectories))
            {
                Types.AddRange(AssemblyLoadContext.Default.LoadFromAssemblyPath(file).GetTypes());
            }
        }
    }
}
