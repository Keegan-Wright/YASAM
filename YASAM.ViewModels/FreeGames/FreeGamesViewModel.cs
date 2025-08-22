using System.Collections.ObjectModel;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using SukiUI.Toasts;
using YASAM.SteamInterface;

namespace YASAM.ViewModels;

public partial class FreeGamesViewModel : PageViewModelBase
{
    private readonly ISteamStoreClient _steamStoreClient;

    [ObservableProperty]
    private ObservableCollection<GameViewModel> _freeGames = [];
    
    [ObservableProperty] private bool _loading;
    
    
    [ObservableProperty] private DateTimeOffset? _lastUpdated;
    
    public sealed override string DisplayName { get; init; }
    
    public FreeGamesViewModel(ISteamStoreClient steamStoreClient)
    {

        _steamStoreClient = steamStoreClient;
        DisplayName = "Free Games";
    }
    
    [RelayCommand]
    private void ShowInSteam(GameViewModel vm)
    {
        _steamStoreClient.OpenStorePage(vm.AppId, vm.Name);
    }
    
    [RelayCommand]
    private async Task LoadAsync()
    {
        Loading = true;
        if (!LastUpdated.HasValue || DateTimeOffset.UtcNow - LastUpdated?.UtcDateTime > TimeSpan.FromMinutes(5))
        {
            
            var freeGames = new List<GameViewModel>();
            await foreach (var freeGame in _steamStoreClient.GetFreeGamesAsync())
            {
                freeGames.Add(new GameViewModel(freeGame.AppId, freeGame!.Name!, 0));
            }
        
            FreeGames = new ObservableCollection<GameViewModel>(freeGames);

            if (freeGames.Any())
            {
                var toastManager = Ioc.Default.GetRequiredService<ISukiToastManager>();
                toastManager.CreateToast()
                    .OfType(NotificationType.Information)
                    .WithContent($"{freeGames.Count} free game(s) available.")
                    .Dismiss().After(TimeSpan.FromSeconds(3))
                    .Dismiss().ByClicking()
                    .Queue();   
            }
        }
        Loading = false;
    }
}