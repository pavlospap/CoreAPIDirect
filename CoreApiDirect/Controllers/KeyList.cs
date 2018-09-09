using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace CoreApiDirect.Controllers
{
    /// <summary>
    /// Represents a strongly typed list of keys.
    /// </summary>
    /// <typeparam name="TKey">The type of elements in the list.</typeparam>
    [ModelBinder(BinderType = typeof(KeyModelBinder))]
    public class KeyList<TKey> : List<TKey>
    {
    }
}
