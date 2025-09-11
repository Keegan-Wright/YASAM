using CommunityToolkit.Mvvm.ComponentModel;

namespace YASAM.ViewModels;

public sealed partial class GameIdleAutomationViewModel : ViewModelBase
{
    [ObservableProperty] private ulong _appId;

    [ObservableProperty] private string _imageUrl;

    [ObservableProperty] private string _name;

    [ObservableProperty] private int _idleFor;

    public GameIdleAutomationViewModel(ulong appId, string name, int idleFor)
    {
        AppId = appId;
        Name = name;
        ImageUrl = $"https://cdn.cloudflare.steamstatic.com/steam/apps/{AppId}/header.jpg";
        IdleFor = idleFor;
    }
}