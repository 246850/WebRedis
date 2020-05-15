using System;
using System.Collections.Generic;
using System.Text;

namespace WebRedis.Models
{
    public class KeyModel
    {
        public int Database { get; set; }
        public string Key { get; set; }
        public string Type { get; set; }
        /// <summary>
        /// 单位：秒， -1：不过期
        /// </summary>
        public long Ttl { get; set; }
        /// <summary>
        /// 存储值 长度
        /// </summary>
        public long ValueSize { get; set; }
    }
}
