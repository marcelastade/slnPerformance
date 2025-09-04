using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Threading.Tasks;
using performance_cache.Model;

namespace performance_cache.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using var connection = new MySqlConnection("Server=localhost;database=fiap;User=root;Password=123");
            await connection.OpenAsync();
            string sql = "select id, name, email from users;";
            var users = await connection.QueryAsync<Users>(sql);
            return Ok(users);
        }
    }
}
