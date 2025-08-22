using System.Collections.ObjectModel;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using SukiUI.Dialogs;
using SukiUI.Toasts;
using YASAM.SteamInterface;
using YASAM.SteamInterface.Models.Internal;

namespace YASAM.ViewModels;

public sealed partial class YourGamesViewModel : PageViewModelBase
{
    private readonly SelectedUserViewModel _selectedUser;

    private readonly ISteamApiClient _steamApiClient;
    private readonly ISteamWorksService _steamWorksService;
    private readonly ISteamStoreClient _steamStoreClient;
    private readonly ISukiDialogManager _dialogManager;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(OwnedGames))] 
    [NotifyPropertyChangedFor(nameof(GameNames))]
    private ObservableCollection<GameViewModel> _games = [];

    [ObservableProperty] private ObservableCollection<GameViewModel> _gamesToDisplay = [];
    


    [ObservableProperty] private DateTimeOffset? _lastUpdated;

    [ObservableProperty] private bool _loading;
    
    [ObservableProperty] private string? _gamesFilterText;

    public YourGamesViewModel(ISteamApiClient steamApiClient, SelectedUserViewModel selectedUser,
        ISteamWorksService steamWorksService, ISteamStoreClient steamStoreClient, ISukiDialogManager dialogManager)
    {
        _selectedUser = selectedUser;
        _steamWorksService = steamWorksService;
        _steamStoreClient = steamStoreClient;
        _dialogManager = dialogManager;
        _steamApiClient = steamApiClient;
        DisplayName = "Your Games";
    }

    public override string DisplayName { get; init; }

    public int OwnedGames => Games.Count;
    public IEnumerable<string> GameNames => Games.Select(x => x.Name);
    

    [RelayCommand]
    private async Task IdleAction(GameViewModel vm)
    {
        var success = await _steamWorksService.IdleGame(new GameToInvoke(vm.AppId, vm.Name));
        
        var toastManager = Ioc.Default.GetRequiredService<ISukiToastManager>();
        if (!success)
            toastManager.CreateToast()
                .OfType(NotificationType.Error)
                .WithTitle("Error").WithContent("Failed to idle game.")
                .Dismiss().After(TimeSpan.FromSeconds(3))
                .Dismiss().ByClicking()
                .Queue();
        else
            toastManager.CreateToast()
                .OfType(NotificationType.Success)
                .WithTitle("Success").WithContent("Idling game.")
                .Dismiss().After(TimeSpan.FromSeconds(3))
                .Dismiss().ByClicking()
                .Queue();

    }


    [RelayCommand]
    private void ShowAchievements(GameViewModel vm)
    {
        var achievementsViewModel = Ioc.Default.GetRequiredService<GameAchievementsViewModel>();
        achievementsViewModel.AppId = vm.AppId;

        _dialogManager.CreateDialog()
            .WithViewModel(s =>
            {
                s.ShowCardBackground = true;
                s.CanDismissWithBackgroundClick = true;
                return achievementsViewModel;
            })
            .TryShow();
    }

    [RelayCommand]
    public void ShowInSteam(GameViewModel vm)
    {
        _steamStoreClient.OpenStorePage(vm.AppId, vm.Name);
    }


    [RelayCommand]
    private async Task LoadAsync()
    {
        Loading = true;
        if (!LastUpdated.HasValue || DateTimeOffset.UtcNow - LastUpdated?.UtcDateTime > TimeSpan.FromMinutes(5))
        {
            var games = _steamApiClient.GetGamesAsync(_selectedUser.SteamUserId!.Value, _selectedUser.ApiKey!);
            var gameVMs = new List<GameViewModel>();
            await foreach (var game in games)
                gameVMs.Add(new GameViewModel(game.AppId!.Value, game.Name!, game.PlaytimeForever!.Value));
            Games = new ObservableCollection<GameViewModel>(gameVMs.OrderBy(x => x.Name));
            LastUpdated = DateTimeOffset.UtcNow;
            GamesToDisplay = Games;
        }
        Loading = false;
    }

    [RelayCommand]
    private void UpdateFilteredGames(string filter)
    {
        if (string.IsNullOrEmpty(filter))
        {
            GamesToDisplay = Games;
        }
        var filteredGames = Games.Where(x => x.Name.Contains(filter, StringComparison.OrdinalIgnoreCase));
        GamesToDisplay = new ObservableCollection<GameViewModel>(filteredGames);
    }
}