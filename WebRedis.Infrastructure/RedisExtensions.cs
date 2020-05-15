using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebRedis.Infrastructure
{
    public static class RedisExtensions
    {
        /// <summary>
        /// 获取键 数据大小长度
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long GetValueSize(this IDatabase db, string key)
        {
            RedisType type = db.KeyType(key);
            switch (type)
            {
                case RedisType.String:
                    return db.StringLength(key);
                case RedisType.List:
                    return db.ListLength(key);
                case RedisType.Set:
                    return db.SetLength(key);
                case RedisType.SortedSet:
                    return db.SortedSetLength(key);
                case RedisType.Hash:
                    return db.HashLength(key);
                case RedisType.Stream:
                    return db.StreamLength(key);
                default:
                    return 0;
            }
        }
    }
}
