using CommunityToolkit.Mvvm.ComponentModel;

namespace YASAM.ViewModels;

public partial class YourGamesViewModel : PageViewModelBase
{
    [ObservableProperty]
    private int _installedGames;
    
    [ObservableProperty]
    private int _ownedGames;
}