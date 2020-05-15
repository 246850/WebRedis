using System;
using System.Collections.Generic;
using System.Text;

namespace WebRedis.Infrastructure
{
    public class PaginationData<T> where T : class
    {
        public PaginationData() : this(null, 0)
        {

        }
        public PaginationData(IEnumerable<T> list, int total)
        {
            List = list;
            Total = total;
        }
        public IEnumerable<T> List { get; set; }
        public int Total { get; set; }
    }
}
