using System;
using System.Collections.Generic;
using System.Text;

namespace WebRedis.Infrastructure
{
    public static class PaginationExtensions
    {
        public static PaginationData<T> ToPagination<T>(this IEnumerable<T> source, int total) where T:class
        {
            return new PaginationData<T>(source, total);
        }
    }
}
