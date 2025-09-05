using Microsoft.EntityFrameworkCore;
using YASAM.Data;
using YASAM.Data.Models;

namespace YASAM.Services.Client;

public class UserService : IUserService
{
    private readonly IDbContextFactory<YasamDbContext> _dbContextFactory;

    public UserService(IDbContextFactory<YasamDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }


    public async Task<TrackedSteamUser> GetSteamUserAsync(Guid id)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        
        return await db.Users.FindAsync(id) ?? throw new NullReferenceException("User not found");
    }

    public async IAsyncEnumerable<TrackedSteamUser> GetTrackedUsersAsync()
    {
        
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        await foreach (var user in db.Users.AsAsyncEnumerable())
        {
            yield return user;
        }
    }

    public async Task<TrackedSteamUser> AddTrackedUserAsync(string name, ulong steamUserId, string apiKey)
    {
        
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        var newUser = new TrackedSteamUser(steamUserId, name, apiKey);
        var user = db.Users.Add(newUser);
        await db.SaveChangesAsync();
        return user.Entity;
    }
}