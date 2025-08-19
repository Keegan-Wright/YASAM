using System.Collections.ObjectModel;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using SukiUI.Toasts;
using YASAM.SteamInterface;
using YASAM.SteamInterface.Models.Internal;

namespace YASAM.ViewModels;

public partial class IdlingGamesViewModel : PageViewModelBase, IGameCardConsumer
{
    private readonly ISteamWorksService _steamWorksService;

    [ObservableProperty] private ObservableCollection<GameViewModel> _idlingGames = [];

    [ObservableProperty] private bool _loading;

    public IdlingGamesViewModel(ISteamWorksService steamWorksService)
    {
        _steamWorksService = steamWorksService;
        DisplayName = "Idling Games";
    }

    public sealed override string DisplayName { get; init; }

    public void IdleActionClicked(GameViewModel vm)
    {
        var success = _steamWorksService.StopIdlingGame(new GameToInvoke(vm.AppId, vm.Name));
        
        var toastManager = Ioc.Default.GetRequiredService<ISukiToastManager>();
        if (!success)
            toastManager.CreateToast()
                .OfType(NotificationType.Error)
                .WithTitle("Error").WithContent("Failed to stop idling game.")
                .Dismiss().After(TimeSpan.FromSeconds(3))
                .Dismiss().ByClicking()
                .Queue();
        else
        {
            IdlingGames.Remove(vm);
            toastManager.CreateToast()
                .OfType(NotificationType.Success)
                .WithTitle("Success").WithContent("Idling stopped.")
                .Dismiss().After(TimeSpan.FromSeconds(3))
                .Dismiss().ByClicking()
                .Queue();
        }

    }

    public void ShowAchievements(GameViewModel vm)
    {
    }

    [RelayCommand]
    private Task LoadAsync()
    {
        Loading = true;
        IdlingGames.Clear();

        var gameVMs = new List<GameViewModel>();
        foreach (var game in _steamWorksService.GetIdlingGames())
            gameVMs.Add(new GameViewModel(game.AppId, game.GameName, 0));
        IdlingGames = new ObservableCollection<GameViewModel>(gameVMs);


        Loading = false;

        return Task.CompletedTask;
    }
}