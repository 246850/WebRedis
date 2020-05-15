using System;
using System.Collections.Generic;
using System.Text;

namespace WebRedis.Models
{
    public class RedisValueModel<T>
    {
        public int Database { get; set; }
        public string Key { get; set; }
        public long TTL { get; set; }
        public T Data { get; set; }
    }
}
