using Microsoft.EntityFrameworkCore;
using TickerQ.EntityFrameworkCore.Configurations;
using TickerQ.EntityFrameworkCore.Entities;
using YASAM.Data.Models;

namespace YASAM.Data;

public class YasamDbContext : DbContext
{
    public DbSet<TrackedSteamUser> Users { get; set; }
    
    public DbSet<CronTickerEntity> CronTickers { get; set; }
    public DbSet<TimeTickerEntity> TimeTickers { get; set; }
    public DbSet<CronTickerOccurrenceEntity<CronTickerEntity>> CronTickerOccurrences { get; set; }
    
    //public DbSet<AutomaticIdlingConfiguration> AutomaticIdlingConfigurations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new TimeTickerConfigurations());  
        modelBuilder.ApplyConfiguration(new CronTickerConfigurations()); 
        modelBuilder.ApplyConfiguration(new CronTickerOccurrenceConfigurations()); 

    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=\"YASAM.db\"",
            sqliteOptions => { sqliteOptions.MigrationsAssembly("YASAM.Data"); });
    }
}