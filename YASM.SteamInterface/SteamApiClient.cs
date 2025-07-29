namespace YASM.SteamInterface;

internal class SteamApiClient : HttpClient, ISteamApiClient
{
    private readonly HttpClient _client;
    
    public SteamApiClient(HttpClient client)
    {
        _client = client;
    }

    public IAsyncEnumerable<Achievement> LoadAchivementsByAppId(string appId)
    {
        throw new NotImplementedException();
    }
}