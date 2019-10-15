using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EasyCaching.Core;
using Microsoft.EntityFrameworkCore;

namespace BService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BController : ControllerBase
    {
        private readonly ILogger<BController> _logger;
        private readonly IEasyCachingProviderFactory _cacheProviderFactory;
        private readonly BDbContext _dbContext;

        public BController(ILogger<BController> logger, IEasyCachingProviderFactory cacheProviderFactory, BDbContext context)
        {
            _logger = logger;
            _cacheProviderFactory = cacheProviderFactory;
            _dbContext = context;

            if (_dbContext.Database.EnsureCreated())
            {
                _dbContext.DemoObjs.AddRange(new List<DemeObj>()
                {
                    new DemeObj() {Id = 1, Name = "Jack"},
                    new DemeObj() {Id = 2, Name = "Kobe"},
                    new DemeObj() {Id = 3, Name = "Catcher"},
                });
                _dbContext.SaveChanges();
            }
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var cache = _cacheProviderFactory.GetCachingProvider("m1");

            var obj = await cache.GetAsync("mykey", async () => await _dbContext.DemoObjs.ToListAsync(), TimeSpan.FromSeconds(30));

            return Ok(obj);
        }
    }
}
