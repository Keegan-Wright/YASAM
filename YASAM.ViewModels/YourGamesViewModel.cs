using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using YASM.SteamInterface;

namespace YASAM.ViewModels;

public sealed partial class YourGamesViewModel : PageViewModelBase
{
    
    public override string DisplayName { get; init; }

    private readonly ISteamWorksService _steamWorksService;


    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(InstalledGames))]
    [NotifyPropertyChangedFor(nameof(OwnedGames))]
    private ObservableCollection<GameViewModel> _games = [];


    public int InstalledGames => Games.Count(x => x.Installed);


    public int OwnedGames => Games.Count;
    
    [ObservableProperty]
    private DateTimeOffset? _lastUpdated;

    public YourGamesViewModel(ISteamWorksService steamWorksService)
    {
        _steamWorksService = steamWorksService;
        DisplayName = "Your Games";
    }


    [RelayCommand]
    private async Task LoadAsync()
    {
        if (!LastUpdated.HasValue || (DateTimeOffset.UtcNow - LastUpdated?.UtcDateTime > TimeSpan.FromMinutes(5)))
        {
            var games  = _steamWorksService.GetGames();
           var gameVMs = new List<GameViewModel>();
            await foreach (var game in games)
            {
                gameVMs.Add(new(game.AppId, game.Name, game.Installed, game.ImgUrl, game.PlaytimeTotal));
            }
            Games = new ObservableCollection<GameViewModel>(gameVMs.OrderBy(x => x.Name));
            LastUpdated = DateTimeOffset.UtcNow;
        }
    }

}