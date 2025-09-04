using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Threading.Tasks;
using performance_cache.Model;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace performance_cache.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string key = "get-users";
            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            IDatabase db = redis.GetDatabase();
            await db.KeyExpireAsync(key, TimeSpan.FromSeconds(20));
            string userValue = await db.StringGetAsync(key);

            if(!string.IsNullOrEmpty(userValue))
            {
                return Ok(userValue);
            }

            using var connection = new MySqlConnection("Server=localhost;database=fiap;User=root;Password=123");
            await connection.OpenAsync();
            string sql = "select id, name, email from users;";
            var users = await connection.QueryAsync<Users>(sql);
            var userJson = JsonConvert.SerializeObject(users);
            await db.StringSetAsync(key, userJson);
            Thread.Sleep(3000);
            return Ok(users);
        }
    }
}
