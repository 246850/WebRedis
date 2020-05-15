using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using WebRedis.Infrastructure;
using WebRedis.Models;

namespace WebRedis.Web.Controllers
{
    public class RedisController : Controller
    {
        private IConnectionMultiplexer _connection;
        public RedisController(IHttpContextAccessor accessor)
        {
            // 构造函数时 当前HttpContext为空，需通过IHttpContextAccessor来获取
            _connection = accessor.HttpContext.GetConnectionMultiplexer();
        }
        #region Database 相关
        /// <summary>
        /// 数据库列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<DatabaseModel> Databases()
        {
            IServer server = _connection.GetServer(_connection.Configuration);

            List<DatabaseModel> root = new List<DatabaseModel>();
            var serverModel = new DatabaseModel { Id = 0, Title = "Redis服务器", IconClass = "fa fa-server", Children= new List<DatabaseModel>() };
            root.Add(serverModel);
            List<DatabaseModel> database = new List<DatabaseModel>();
            for (int i = 0; i < server.DatabaseCount; i++)
            {
                var keys = server.Keys(i);
                long size = server.DatabaseSize(i);
                int keyCount = keys.Count();
                database.Add(new DatabaseModel { Id = i, Title = $"db{i}({keyCount})", KeyCount = keyCount, Size = size, IconClass="fa fa-database", Href = $"/redis/keys?database={i}", });
            }
            serverModel.Children.AddRange(database);
            return root;
        }
        /// <summary>
        /// 清空数据库 - 单个
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task Flush(int database)
        {
            IServer server = _connection.GetServer(_connection.Configuration);
            await server.FlushDatabaseAsync(database);
        }
        /// <summary>
        /// 清空数据库 - 所有
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task FlushAll()
        {
            IServer server = _connection.GetServer(_connection.Configuration);
            await server.FlushAllDatabasesAsync();
        }
        #endregion

        #region Key 相关
        [HttpGet]
        public IActionResult Keys(int database)
        {
            ViewBag.database = database;
            return View();
        }
        /// <summary>
        /// 键列表
        /// </summary>
        /// <param name="database"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpPost]
        public PaginationData<KeyModel> Keys(int database, int page = 1, int pageSize = 30)
        {
            IServer server = _connection.GetServer(_connection.Configuration);
            IDatabase db = _connection.GetDatabase(database);
            var keys = server.Keys(database);
            return keys.OrderBy(x=> x.ToString()).Skip((page - 1) * pageSize).Take(pageSize).Select(item =>
            {
                string key = item.ToString();
                var timespan = db.KeyTimeToLive(key);
                return new KeyModel
                {
                    Database = database,
                    Key = key,
                    Type = db.KeyType(key).ToString(),
                    Ttl = timespan.HasValue ? Convert.ToInt64(timespan.Value.TotalSeconds) : -1,
                    ValueSize = db.GetValueSize(key)
                };
            }).ToPagination(keys.Count());
        }
        /// <summary>
        /// 删除 Key
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task KeyDelete(int database, string key)
        {
            IDatabase db = _connection.GetDatabase(database);
            await db.KeyDeleteAsync(key);
        }
        /// <summary>
        /// 过期时间
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task Expire(int database, string key, int value = -1)
        {
            IDatabase db = _connection.GetDatabase(database);
            if (value <= 0)
            {
                await db.KeyExpireAsync(key, (TimeSpan?)null);
            }
            else
            {
                await db.KeyExpireAsync(key, new TimeSpan(0, 0, value));
            }
        }
        /// <summary>
        /// 重命名
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task Rename(int database, string key, string value)
        {
            IDatabase db = _connection.GetDatabase(database);
            await db.KeyRenameAsync(key, value);
        }
        #endregion

        #region Value 相关
        /// <summary>
        /// String类型 读
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> StringValue(int database, string key)
        {
            IDatabase db = _connection.GetDatabase(database);
            var value = await db.StringGetAsync(key);
            var timespan = db.KeyTimeToLive(key);
            RedisValueModel<string> model = new RedisValueModel<string>
            {
                Database = database,
                Key = key,
                TTL = timespan.HasValue ? Convert.ToInt64(timespan.Value.TotalSeconds) : -1,
                Data = value
            };
            return View(model);
        }
        /// <summary>
        /// String类型 写
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task StringSet(int database, string key, string value)
        {
            IDatabase db = _connection.GetDatabase(database);
            await db.StringSetAsync(key, value);
        }

        /// <summary>
        /// List类型 读
        /// </summary>
        [HttpGet]
        public async Task<List<string>> ListValue(int database, string key)
        {
            IDatabase db = _connection.GetDatabase(database);
            var values = await db.ListRangeAsync(key);
            return values.Select(item => item.ToString()).ToList();
        }
        /// <summary>
        /// List类型 写
        /// </summary>
        [HttpGet]
        public async Task ListRightPush(int database, string key, string value)
        {
            IDatabase db = _connection.GetDatabase(database);
            await db.ListRightPushAsync(key, value);
        }

        /// <summary>
        /// Set类型 读 - 无序集合
        /// </summary>
        [HttpGet]
        public async Task<List<string>> SetValue(int database, string key)
        {
            IDatabase db = _connection.GetDatabase(database);

            var values = await db.SetMembersAsync(key);
            return values.Select(item => item.ToString()).ToList();
        }
        /// <summary>
        /// Set类型 写 - 无序集合
        /// </summary>
        [HttpGet]
        public async Task SetAdd(int database, string key, string value)
        {
            IDatabase db = _connection.GetDatabase(database);
            await db.SetAddAsync(key, value);
        }

        /// <summary>
        /// ZSet类型 读 - 有序集合
        /// </summary>
        /// <param name="database"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<RedisEntryModel>> SortSetValue(int database, string key)
        {
            IDatabase db = _connection.GetDatabase(database);

            var values = await db.SortedSetRangeByRankWithScoresAsync(key);
            return values.Select(item => new RedisEntryModel
            {
                Score = item.Score,
                Value = item.Element.ToString()
            }).ToList();
        }
        /// <summary>
        /// ZSet类型 写 - 有序集合
        /// </summary>
        [HttpGet]
        public async Task SortSetAdd(int database, string key, string value, double score)
        {
            IDatabase db = _connection.GetDatabase(database);
            await db.SortedSetAddAsync(key, value, score);
        }

        /// <summary>
        /// Hash类型 读
        /// </summary>
        [HttpGet]
        public async Task<List<RedisEntryModel>> HashValue(int database, string key)
        {
            IDatabase db = _connection.GetDatabase(database);

            var values = await db.HashGetAllAsync(key);
            return values.Select(item => new RedisEntryModel
            {
                Key = item.Name,
                Value = item.Value
            }).ToList();
        }
        /// <summary>
        /// Hash类型 写
        /// </summary>
        [HttpGet]
        public async Task HashSet(int database, string key, string field, string value)
        {
            IDatabase db = _connection.GetDatabase(database);
            await db.HashSetAsync(key, field, value);
        }
        #endregion
    }
}