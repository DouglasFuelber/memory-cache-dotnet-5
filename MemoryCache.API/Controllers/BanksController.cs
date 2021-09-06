using MemoryCache.API.Models;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string brazilBanksApiUrl = "https://brasilapi.com.br/api/banks/v1";

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(brazilBanksApiUrl);
                var responseData = await response.Content.ReadAsStringAsync();

                var banks = JsonSerializer.Deserialize<List<Bank>>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return Ok(banks);
            }
        }
    }
}
