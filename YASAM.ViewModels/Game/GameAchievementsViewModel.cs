using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using YASAM.SteamInterface;

namespace YASAM.ViewModels;

public partial class GameAchievementsViewModel : ViewModelBase
{
    
    private readonly ISteamApiClient  _steamApiClient;
    private readonly ISteamWorksService _steamWorksService;
    private readonly SelectedUserViewModel _selectedUserViewModel;
    
    [ObservableProperty]
    private ObservableCollection<GameAchievementViewModel> _achievements = [];

    [ObservableProperty]
    private bool _loading;

    public GameAchievementsViewModel(ISteamApiClient steamApiClient, ISteamWorksService steamWorksService, SelectedUserViewModel selectedUserViewModel)
    {
        _steamApiClient = steamApiClient;
        _steamWorksService = steamWorksService;
        _selectedUserViewModel = selectedUserViewModel;
    }


    [RelayCommand]
    public async void LoadAchievements()
    {
        Loading = true;
        var games  = _steamApiClient.GetAchievements(_selectedUserViewModel.SteamUserId, _selectedUserViewModel.ApiKey, AppId);
        var gameVMs = new List<GameViewModel>();
        await foreach (var game in games)
        {
            gameVMs.Add(new(game.AppId, game.Name, game.PlaytimeForever));
        }
        Games = new ObservableCollection<GameViewModel>(gameVMs.OrderBy(x => x.Name));
        Loading = false;
    }
}