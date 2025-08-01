using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Steamworks;
using YASM.SteamInterface;

namespace YASAM.ViewModels;

public sealed partial class YourGamesViewModel : PageViewModelBase
{
    
    private readonly SelectedUserViewModel _selectedUser;
    public override string DisplayName { get; init; }

    private readonly ISteamApiClient _steamApiClient;


    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(OwnedGames))]
    private ObservableCollection<GameViewModel> _games = [];
    
    public int OwnedGames => Games.Count;
    
    [ObservableProperty]
    private DateTimeOffset? _lastUpdated;

    public YourGamesViewModel(ISteamApiClient steamApiClient, SelectedUserViewModel selectedUser)
    {
        _selectedUser = selectedUser;
        _steamApiClient = steamApiClient;
        DisplayName = "Your Games";
    }


    [RelayCommand]
    private async Task LoadAsync()
    {
        if (!LastUpdated.HasValue || (DateTimeOffset.UtcNow - LastUpdated?.UtcDateTime > TimeSpan.FromMinutes(5)))
        {
            var games  = _steamApiClient.GetGames(_selectedUser.SteamUserId, _selectedUser.ApiKey);
           var gameVMs = new List<GameViewModel>();
            await foreach (var game in games)
            {
                gameVMs.Add(new(game.AppId, game.Name, game.PlaytimeForever));
            }
            Games = new ObservableCollection<GameViewModel>(gameVMs.OrderBy(x => x.Name));
            LastUpdated = DateTimeOffset.UtcNow;
        }
    }

}