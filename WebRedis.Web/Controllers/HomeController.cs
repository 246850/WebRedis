using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebRedis.Domain;
using WebRedis.Infrastructure;
using WebRedis.Models;

namespace WebRedis.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly WebredisContext _dbContext;
        public HomeController(WebredisContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var list = _dbContext.ConnectionConfig.OrderByDescending(x => x.Id).Select(entity => entity.MapTo<ConnectionConfig, ConnectionConfigModel>()).ToList();
            ViewBag.databases = list;
            return View();
        }

        [HttpGet]
        public IActionResult Connections()
        {
            return View();
        }

        [HttpPost]
        public PaginationData<ConnectionConfigModel> Connections(int page, int pageSize)
        {
            var pagination = _dbContext.ConnectionConfig.OrderByDescending(x => x.Id).ToPagination<ConnectionConfig, ConnectionConfigModel>(page, pageSize);
            return pagination;
        }

        [HttpPost]
        public async Task<IActionResult> Add(ConnectionConfigModel model)
        {
            _dbContext.ConnectionConfig.Add(new ConnectionConfig
            {
                Name = model.Name.Trim(),
                IpAddress = model.IpAddress.Trim(),
                Port = model.Port,
                Password = model.Password.Trim(),
                Description = model.Description,
                CreateTime = DateTime.Now
            });
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Connections", "Home");
        }

        [HttpPost]
        public async Task Delete(int id)
        {
            var entity = await _dbContext.ConnectionConfig.FindAsync(id);
            if (entity != null)
            {
                _dbContext.ConnectionConfig.Remove(entity);
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
