using Microsoft.AspNetCore.Http;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebRedis.Infrastructure
{
    public static class HttpContextExtensions
    {
        public static IConnectionMultiplexer GetConnectionMultiplexer(this HttpContext context)
        {
            if (context.Items[ConstantKey.RedisConnectionKey] == null) return null;

            if (context.Items[ConstantKey.RedisConnectionKey] is IConnectionMultiplexer connection) return connection;

            throw new Exception("Redis连接异常");
        }
    }
}
