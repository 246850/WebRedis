using Microsoft.AspNetCore.Http;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebRedis.Infrastructure
{
    public class RedisConnectionMiddleware
    {
        private RequestDelegate _next;

        public RedisConnectionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // 执行前 设置 Redis 连接
            IConnectionMultiplexer connection = ConnectionMultiplexer.Connect("172.16.10.246:6379");
            context.Items[ConstantKey.RedisConnectionKey] = connection;
            try
            {
                await _next.Invoke(context);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                // 执行后释放 Redis 连接
                connection.Dispose();
            }

        }
    }
}
