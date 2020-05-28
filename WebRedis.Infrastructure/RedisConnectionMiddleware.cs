using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRedis.Domain;

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
            var dbContext = (WebredisContext)context.RequestServices.GetService(typeof(WebredisContext));
            string database = context.Request.Cookies["database"];

            // 执行前 设置 Redis 连接
            IConnectionMultiplexer connection = null;
            if (!string.IsNullOrWhiteSpace(database))
            {
                var entity = dbContext.ConnectionConfig.Find(Convert.ToInt32(database));
                if (entity != null)
                {
                    string connectionString = $"{entity.IpAddress}:{entity.Port}";
                    if (!string.IsNullOrWhiteSpace(entity.Password))
                    {
                        connectionString += $",password={entity.Password}";
                    }
                    connection = ConnectionMultiplexer.Connect(connectionString);
                    context.Items[ConstantKey.RedisConnectionKey] = connection;
                }
            }

            
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
                connection?.Dispose();
            }

        }
    }
}
