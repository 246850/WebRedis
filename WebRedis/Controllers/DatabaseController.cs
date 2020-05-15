using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace AspNetCoreRedis.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class DatabaseController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("172.16.10.246:6379"))
            {
                IServer server = redis.GetServer("172.16.10.246:6379");
                List<DatabaseInfo> list = new List<DatabaseInfo>();
                for(int i = 0; i <server.DatabaseCount; i++)
                {
                    var keys = server.Keys(i);
                    var size = server.DatabaseSize(i);
                    list.Add(new DatabaseInfo { Id = i, Name = "db" + i, KeyCount = keys.Count(), Size = size });
                }

                return Ok(list);
            }

        }
    }

    public class DatabaseInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int KeyCount { get; set; }
        public long Size { get; set; }
    }
}