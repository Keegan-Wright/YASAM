using CommunityToolkit.Mvvm.ComponentModel;

namespace YASAM.ViewModels;

public sealed partial class GameViewModel : ViewModelBase
{
    [ObservableProperty]
    private uint _appId;
    
    [ObservableProperty]
    private string _name;
    
    [ObservableProperty]
    private bool _installed;
    
    [ObservableProperty]
    private string _imageUrl;
    
    [ObservableProperty]
    private int _playTime;

    public GameViewModel(uint appId, string name, bool installed, string imageUrl, int playTime)
    {
        AppId = appId;
        Name = name;
        Installed = installed;
        ImageUrl = imageUrl;
        PlayTime = playTime;
    }
}