using H6_WiseWatt_Backend.MySqlData;
using Microsoft.AspNetCore.Mvc;

namespace H6_WiseWatt_Backend.Api.Controllers
{
    [ApiController]
    public class DevController : ControllerBase
    {
        private readonly MySqlDbContext dbContext;

        public DevController(MySqlDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// This method is used for development purpose, and is used to reset the database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/dev/reset")]
        public async Task<IActionResult> ResetDb()
        {
            try
            {
                await this.dbContext.Database.EnsureDeletedAsync();
                await this.dbContext.Database.EnsureCreatedAsync();
                return Ok("Db has been reset");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
