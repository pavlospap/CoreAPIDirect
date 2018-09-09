using System.Collections.Generic;
using System.Linq;
using CoreApiDirect.Entities;

namespace CoreApiDirect.Tests.DataContext
{
    internal class KeySetter
    {
        public static IEnumerable<TEntity> SetKeys<TEntity>(IEnumerable<TEntity> items)
            where TEntity : Entity<int>
        {
            int cnt = 0;
            return items.Select(p =>
            {
                p.Id = ++cnt;
                return p;
            });
        }
    }
}
