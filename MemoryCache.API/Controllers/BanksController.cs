using MemoryCache.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MemoryCache.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BanksController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;
        private const string BANKS_MEMORY_CACHE_KEY = "BanksMemoryCacheKey";

        public BanksController(IMemoryCache memoryCache) {
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string brazilBanksApiUrl = "https://brasilapi.com.br/api/banks/v1";

            if (_memoryCache.TryGetValue(BANKS_MEMORY_CACHE_KEY, out List<Bank> banks))
                return Ok(banks);

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(brazilBanksApiUrl);
                var responseData = await response.Content.ReadAsStringAsync();

                banks = JsonSerializer.Deserialize<List<Bank>>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                _memoryCache.Set(BANKS_MEMORY_CACHE_KEY, banks, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3600),
                    SlidingExpiration = TimeSpan.FromSeconds(1200)
                });

                return Ok(banks);
            }
        }
    }
}
