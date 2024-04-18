using H6_WiseWatt_Backend.MySqlData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
            // Define a converter from TimeSpan to string and from string to TimeSpan
            var timeSpanToStringConverter = new ValueConverter<TimeSpan, string>(
                timeSpan => timeSpan.ToString(@"hh\:mm\:ss"),   // Convert TimeSpan to string
                str => TimeSpan.Parse(str));                   // Convert string to TimeSpan


            modelBuilder.Entity<UserDbModel>().HasKey(u => u.Id);
            modelBuilder.Entity<DeviceDbModel>().HasKey(u => u.Id);
            modelBuilder.Entity<ElectricityPriceDbModel>().HasKey(u => u.Id);

            modelBuilder.Entity<DeviceDbModel>().Property(e => e.DeviceType).HasConversion<string>();
            // Apply the converter to the OnTime and OffTime properties
            modelBuilder.Entity<DeviceDbModel>().Property(p => p.OnTime)
                .HasConversion(timeSpanToStringConverter)
                .HasColumnType("VARCHAR(8)");  // Use VARCHAR or CHAR as appropriate

            modelBuilder.Entity<DeviceDbModel>().Property(p => p.OffTime)
                .HasConversion(timeSpanToStringConverter)
                .HasColumnType("VARCHAR(8)");  // Ensure the column type can store the string format
        

            base.OnModelCreating(modelBuilder);
        }
        #endregion

        #region Properties
        public DbSet<UserDbModel> Users { get; set; }
        public DbSet<DeviceDbModel> Devices { get; set; }
        public DbSet<ElectricityPriceDbModel> ElectricityPrices { get; set; }
        #endregion
    }
}
