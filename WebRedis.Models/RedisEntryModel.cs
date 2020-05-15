using System;
using System.Collections.Generic;
using System.Text;

namespace WebRedis.Models
{
    public class RedisEntryModel
    {
        public string Value { get; set; }
        public double Score { get; set; }
        public string Key { get; set; }
    }
}
