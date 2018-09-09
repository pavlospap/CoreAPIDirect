using System;

namespace CoreApiDirect.Entities
{
    /// <summary>
    /// Marks an entity property so that it will be used for searching in an HTTP GET request when a search parameter exists in the query string.
    /// </summary>
    public class SearchKeyAttribute : Attribute
    {
    }
}
