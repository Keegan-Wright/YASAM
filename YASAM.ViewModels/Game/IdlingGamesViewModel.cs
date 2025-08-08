using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using YASAM.SteamInterface;
using YASAM.SteamInterface.Internal;

namespace YASAM.ViewModels;

public partial class IdlingGamesViewModel : PageViewModelBase, IGameCardConsumer
{
    private readonly ISteamWorksService _steamWorksService;

    public override string DisplayName { get; init; }

    [ObservableProperty] private ObservableCollection<GameViewModel> _idlingGames = [];

    [ObservableProperty] private bool _loading;

    public IdlingGamesViewModel(ISteamWorksService steamWorksService)
    {
        _steamWorksService = steamWorksService;
        DisplayName = "Idling Games";
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        Loading = true;
        IdlingGames.Clear();

        var gameVMs = new List<GameViewModel>();
        foreach (var game in _steamWorksService.GetIdlingGames()) gameVMs.Add(new GameViewModel(game.AppId, game.GameName, 0));
        IdlingGames = new ObservableCollection<GameViewModel>(gameVMs);


        Loading = false;
    }

    public void IdleActionClicked(GameViewModel vm)
    {
        _steamWorksService.StopIdleGame(new GameToInvoke(vm.AppId, vm.Name));
        IdlingGames.Remove(vm);
    }

    public void ShowAchievements(GameViewModel vm)
    {
        var a = 1;
    }
}