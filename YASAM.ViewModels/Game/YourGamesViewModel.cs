using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using SukiUI.Dialogs;
using YASAM.SteamInterface;
using YASAM.SteamInterface.Models.Internal;

namespace YASAM.ViewModels;

public sealed partial class YourGamesViewModel : PageViewModelBase, IGameCardConsumer
{
    private readonly SelectedUserViewModel _selectedUser;

    private readonly ISteamApiClient _steamApiClient;
    private readonly ISteamWorksService _steamWorksService;


    [ObservableProperty] [NotifyPropertyChangedFor(nameof(OwnedGames))]
    private ObservableCollection<GameViewModel> _games = [];

    [ObservableProperty] private DateTimeOffset? _lastUpdated;

    [ObservableProperty] private bool _loading;

    public YourGamesViewModel(ISteamApiClient steamApiClient, SelectedUserViewModel selectedUser,
        ISteamWorksService steamWorksService)
    {
        _selectedUser = selectedUser;
        _steamWorksService = steamWorksService;
        _steamApiClient = steamApiClient;
        DisplayName = "Your Games";
    }

    public override string DisplayName { get; init; }

    public int OwnedGames => Games.Count;

    public void IdleActionClicked(GameViewModel vm)
    {
        _steamWorksService.IdleGame(new GameToInvoke(vm.AppId, vm.Name));
    }


    public void ShowAchievements(GameViewModel vm)
    {
        var achievementsViewModel = Ioc.Default.GetRequiredService<GameAchievementsViewModel>();
        achievementsViewModel.AppId = vm.AppId;

        var dialogManager = Ioc.Default.GetRequiredService<ISukiDialogManager>();
        dialogManager.CreateDialog()
            .WithViewModel(s =>
            {
                s.ShowCardBackground = true;
                s.CanDismissWithBackgroundClick = true;
                return achievementsViewModel;
            })
            .TryShow();
    }


    [RelayCommand]
    private async Task LoadAsync()
    {
        if (!LastUpdated.HasValue || DateTimeOffset.UtcNow - LastUpdated?.UtcDateTime > TimeSpan.FromMinutes(5))
        {
            Loading = true;
            var games = _steamApiClient.GetGames(_selectedUser.SteamUserId!.Value, _selectedUser.ApiKey!);
            var gameVMs = new List<GameViewModel>();
            await foreach (var game in games)
                gameVMs.Add(new GameViewModel(game.AppId!.Value, game.Name!, game.PlaytimeForever!.Value));
            Games = new ObservableCollection<GameViewModel>(gameVMs.OrderBy(x => x.Name));
            LastUpdated = DateTimeOffset.UtcNow;
            Loading = false;
        }
    }
}