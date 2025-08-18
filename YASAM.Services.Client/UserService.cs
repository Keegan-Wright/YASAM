using YASAM.Data;
using YASAM.Data.Models;

namespace YASAM.Services.Client;

public class UserService : IUserService
{
    private readonly YasamDbContext _db;

    public UserService(YasamDbContext db)
    {
        _db = db;
    }


    public async Task<TrackedSteamUser> GetSteamUserAsync(Guid id)
    {
        return await _db.Users.FindAsync(id) ?? throw new NullReferenceException("User not found");
    }

    public IAsyncEnumerable<TrackedSteamUser> GetTrackedUsersAsync()
    {
        return _db.Users.AsAsyncEnumerable();
    }

    public async Task<TrackedSteamUser> AddTrackedUserAsync(string name, ulong steamUserId, string apiKey)
    {
        var newUser = new TrackedSteamUser(steamUserId, name, apiKey);
        var user = _db.Users.Add(newUser);
        await _db.SaveChangesAsync();
        return user.Entity;
    }
}