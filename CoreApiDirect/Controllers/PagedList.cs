using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreApiDirect.Url;
using Microsoft.EntityFrameworkCore;

namespace CoreApiDirect.Controllers
{
    internal class PagedList<TEntity> : List<TEntity>
    {
        public int TotalCount { get; }
        public int PageNumber { get; }
        public int PageSize { get; }
        public int TotalPages { get; }

        public bool HasPrevious
        {
            get
            {
                return PageNumber > 1;
            }
        }

        public bool HasNext
        {
            get
            {
                return PageNumber < TotalPages;
            }
        }

        private PagedList(
            IEnumerable<TEntity> items,
            int totalCount,
            int pageNumber,
            int pageSize)
        {
            AddRange(items);
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling((decimal)(totalCount / pageSize));
        }

        public static async Task<PagedList<TEntity>> CreateAsync(IQueryable<TEntity> query, QueryString queryString)
        {
            var totalCount = query.Count();
            var items = await query
                .Skip((queryString.PageNumber - 1) * queryString.PageSize)
                .Take(queryString.PageSize)
                .ToListAsync();

            return new PagedList<TEntity>(items, totalCount, queryString.PageNumber, queryString.PageSize);
        }
    }
}
