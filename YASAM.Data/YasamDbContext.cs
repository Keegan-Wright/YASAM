using Microsoft.EntityFrameworkCore;
using YASAM.Data.Models;

namespace YASAM.Data;

public class YasamDbContext : DbContext
{
    public DbSet<TrackedSteamUser> Users { get; set; }
    

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source=\"YASAM.db\"", sqliteOptions =>
        {
            sqliteOptions.MigrationsAssembly("YASAM.Data");
        });

    }
}