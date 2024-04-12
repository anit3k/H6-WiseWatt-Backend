using H6_WiseWatt_Backend.MySqlData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace H6_WiseWatt_Backend.MySqlData
{
    public class MySqlDbContext : DbContext
    {
        private readonly IConfiguration configuration;
        public MySqlDbContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var _connectionString = configuration.GetConnectionString("MyConnectionString");
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(connectionString: _connectionString, serverVersion: ServerVersion.AutoDetect(_connectionString));
            }
        }
        public DbSet<UserDbModel> Users { get; set; }
    }
}
