using System.Collections.Frozen;
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
    
    [ObservableProperty]
    private ulong _appId;

    public GameAchievementsViewModel(ISteamApiClient steamApiClient, ISteamWorksService steamWorksService, SelectedUserViewModel selectedUserViewModel)
    {
        _steamApiClient = steamApiClient;
        _steamWorksService = steamWorksService;
        _selectedUserViewModel = selectedUserViewModel;
    }


    [RelayCommand]
    private async Task LoadAsync()
    {
        Loading = true;
        
        var achievements  = _steamApiClient.GetAchievements(_selectedUserViewModel.SteamUserId, _selectedUserViewModel.ApiKey, AppId);
        var achievementsVMs = new List<GameAchievementViewModel>();
        
        await foreach (var achievement in achievements)
        {
            achievementsVMs.Add(new(achievement.ApiName, achievement.Name, achievement.Description, achievement.Achieved == 1, achievement.Hidden == 1, achievement.NotAchievedIcon, achievement.AchievedIcon, achievement.Achieved == 1 ? DateTimeOffset.FromUnixTimeSeconds(achievement.UnlockTime).Date : null));
        }
        
        Achievements = new ObservableCollection<GameAchievementViewModel>(achievementsVMs.OrderBy(x => x.DisplayName));
        Loading = false;
    }

    [RelayCommand]
    private async Task SaveChangesAsync()
    {
        var achievementsToToggle = Achievements.Where(x => x.ShouldToggle).ToFrozenSet();
        if (achievementsToToggle.Any())
        {
            await _steamWorksService.LockAchievements(AppId, achievementsToToggle.Where(x => x.Achieved).Select(x => x.Id));
            await _steamWorksService.UnlockAchievements(AppId, achievementsToToggle.Where(x => !x.Achieved).Select(x => x.Id));
        }
    }
    
    [RelayCommand]
    private async Task UnlockAllAchievementsAsync()
    {
        await _steamWorksService.UnlockAllAchievements(AppId);
    }
    
        
    [RelayCommand]
    private async Task LockAllAchievementsAsync()
    {
        await _steamWorksService.LockAllAchievements(AppId);
    }
    
}