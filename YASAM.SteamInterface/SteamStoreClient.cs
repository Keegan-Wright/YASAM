using HtmlAgilityPack;
using YASAM.SteamInterface.Models.Api;

namespace YASAM.SteamInterface;

public class SteamStoreClient : HttpClient, ISteamStoreClient
{
    private readonly HttpClient _client;

    public SteamStoreClient(HttpClient client)
    {
        _client = client;
    }

    public async IAsyncEnumerable<SteamFreeGame> GetFreeGamesAsync()
    {
        var responseBody =
            await _client.GetAsync("search/?l=english&maxprice=free&specials=1&category1=998");

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(await responseBody.Content.ReadAsStringAsync());
        
        var nodes = htmlDoc.DocumentNode.SelectNodes(
            "/html/body/div[1]/div[7]/div[6]/form/div[1]/div/div[1]/div[3]/div[2]/div[3]/a");
        
        foreach (var node in nodes)
        {
            if (node.Attributes.Any(x => x.Name == "data-ds-appid"))
            {
                var elementHtmlDoc = new HtmlDocument();
                elementHtmlDoc.LoadHtml(node.InnerHtml);
                var titleNode =  elementHtmlDoc.DocumentNode.SelectSingleNode("div[2]/div[1]/span");
                
                var appId = node.Attributes["data-ds-appid"].Value;
                yield return new SteamFreeGame(ulong.Parse(appId), titleNode!.InnerText.Trim());
            }
        }
    }
}