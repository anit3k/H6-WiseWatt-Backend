using H6_WiseWatt_Backend.MySqlData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace H6_WiseWatt_Backend.MySqlData
{
    public class MySqlDbContext : DbContext
    {
        #region fields
        private readonly IConfiguration configuration;
        #endregion

        #region Constructor
        public MySqlDbContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        #endregion

        #region Methods
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var _connectionString = configuration.GetConnectionString("MyConnectionString");
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(connectionString: _connectionString, serverVersion: ServerVersion.AutoDetect(_connectionString));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure primary keys
            modelBuilder.Entity<UserDbModel>().HasKey(u => u.Id);
            modelBuilder.Entity<DeviceDbModel>().HasKey(d => d.Id);
            modelBuilder.Entity<UserDeviceDbModel>().HasKey(ud => ud.Id);

            // Configure relations
            modelBuilder.Entity<UserDeviceDbModel>()
                .HasOne(ud => ud.User)
                .WithMany(u => u.UserDevices)
                .HasForeignKey(ud => ud.UserId);

            modelBuilder.Entity<UserDeviceDbModel>()
                .HasOne(ud => ud.Device)
                .WithMany(d => d.UserDevices)
                .HasForeignKey(ud => ud.DeviceId);

            base.OnModelCreating(modelBuilder);
        }
        #endregion

        #region Properties
        public DbSet<UserDbModel> Users { get; set; }
        public DbSet<DeviceDbModel> Devices { get; set; }
        public DbSet<UserDeviceDbModel> UserDevices { get; set; }
        #endregion
    }
}
