using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Db;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GoodsController : ControllerBase
    {
        private readonly DbLocalContext dbContext;

        public GoodsController(DbLocalContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IEnumerable<Goods> GetGoods(string query = null)
        {
            return  dbContext.Goods.AsEnumerable()
                .Where(x => x.Name.ToLower().Contains(query?.ToLower() ?? string.Empty))
                .ToArray();
        }
    }
}