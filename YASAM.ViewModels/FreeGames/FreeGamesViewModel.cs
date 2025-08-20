using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using YASAM.SteamInterface;

namespace YASAM.ViewModels;

public partial class FreeGamesViewModel : PageViewModelBase
{
    private readonly ISteamStoreClient _steamStoreClient;

    [ObservableProperty]
    private ObservableCollection<GameViewModel> _freeGames = [];
    
    [ObservableProperty] private bool _loading;
    public sealed override string DisplayName { get; init; }
    
    public FreeGamesViewModel(ISteamStoreClient steamStoreClient)
    {

        _steamStoreClient = steamStoreClient;
        DisplayName = "Free Games";
    }
    
    [RelayCommand]
    private async Task LoadAsync()
    {
        Loading = true;
        
        var freeGames = new List<GameViewModel>();
        await foreach (var freeGame in _steamStoreClient.GetFreeGamesAsync())
        {
            freeGames.Add(new GameViewModel(freeGame.AppId, freeGame!.Name!, 0));
        }
        
        FreeGames = new ObservableCollection<GameViewModel>(freeGames);
        
        Loading = false;
    }
}