using CommunityToolkit.Mvvm.ComponentModel;

namespace YASAM.ViewModels;

public sealed partial class GameViewModel : ViewModelBase
{
    [ObservableProperty]
    private ulong _appId;
    
    [ObservableProperty]
    private string _name;
    
    [ObservableProperty]
    private string _imageUrl;
    
    [ObservableProperty]
    private int _playTime;

    public GameViewModel(ulong appId, string name, int playTime)
    {
        AppId = appId;
        Name = name;
        ImageUrl = $"https://cdn.cloudflare.steamstatic.com/steam/apps/{AppId}/header.jpg";
        PlayTime = playTime;
    }
}