using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AController : ControllerBase
    {
        private readonly ILogger<AController> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public AController(ILogger<AController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        // GET api/values
        [HttpGet]
        public async Task<string> GetAsync()
        {
            var res = await GetDemoAsync();
            return res;
        }

        private async Task<string> GetDemoAsync()
        {
            var client = _clientFactory.CreateClient();

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://localhost:44345/b")
            };

            var response = await client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadAsStringAsync();

            return body;
        }
    }
}
