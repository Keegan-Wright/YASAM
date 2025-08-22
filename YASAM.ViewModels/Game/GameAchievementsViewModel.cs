using System.Collections.Frozen;
using System.Collections.ObjectModel;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using SukiUI.Toasts;
using YASAM.SteamInterface;

namespace YASAM.ViewModels;

public partial class GameAchievementsViewModel : ViewModelBase
{
    private readonly SelectedUserViewModel _selectedUserViewModel;

    private readonly ISteamApiClient _steamApiClient;
    private readonly ISteamWorksService _steamWorksService;

    [NotifyPropertyChangedFor(nameof(AchievementNames))]
    [ObservableProperty] private ObservableCollection<GameAchievementViewModel> _achievements = [];

    [ObservableProperty] private ulong _appId;

    [ObservableProperty] private bool _loading;
    
    [ObservableProperty] private ObservableCollection<GameAchievementViewModel> _achievementsToDisplay = [];
    
    
    [ObservableProperty] private string? _achievementsFilterText;

    
    public IEnumerable<string> AchievementNames => Achievements.Select(x => x.DisplayName);
    public GameAchievementsViewModel(ISteamApiClient steamApiClient, ISteamWorksService steamWorksService,
        SelectedUserViewModel selectedUserViewModel)
    {
        _steamApiClient = steamApiClient;
        _steamWorksService = steamWorksService;
        _selectedUserViewModel = selectedUserViewModel;
    }


    [RelayCommand]
    private async Task LoadAsync()
    {
        Loading = true;

        var achievements = _steamApiClient.GetAchievementsAsync(_selectedUserViewModel.SteamUserId!.Value,
            _selectedUserViewModel.ApiKey!, AppId);
        var achievementsVMs = new List<GameAchievementViewModel>();

        await foreach (var achievement in achievements)
            achievementsVMs.Add(new GameAchievementViewModel(achievement.ApiName, achievement.Name!,
                achievement.Description!, achievement.Achieved == 1, achievement.Hidden == 1,
                achievement.NotAchievedIcon!, achievement.AchievedIcon!,
                achievement.Achieved == 1
                    ? DateTimeOffset.FromUnixTimeSeconds(achievement.UnlockTime!.Value).Date
                    : null));

        Achievements = new ObservableCollection<GameAchievementViewModel>(achievementsVMs.OrderBy(x => x.DisplayName));
        AchievementsToDisplay = Achievements;
        Loading = false;
    }

    [RelayCommand]
    private async Task SaveChangesAsync()
    {
        var achievementsToToggle = Achievements.Where(x => x.ShouldToggle).ToFrozenSet();
        if (achievementsToToggle.Any())
        {
            var lockSuccess = await _steamWorksService.LockAchievements(AppId,
                achievementsToToggle.Where(x => x.Achieved).Select(x => x.Id));
            var unlockSuccess = await _steamWorksService.UnlockAchievements(AppId,
                achievementsToToggle.Where(x => !x.Achieved).Select(x => x.Id));
            
            await ReloadAndNotifyUser(lockSuccess && unlockSuccess);
        }
    }

    [RelayCommand]
    private async Task UnlockAllAchievementsAsync()
    {
        var success = await _steamWorksService.UnlockAllAchievements(AppId);
        await ReloadAndNotifyUser(success);
    }


    [RelayCommand]
    private async Task LockAllAchievementsAsync()
    {
       var success =  await _steamWorksService.LockAllAchievements(AppId);

       await ReloadAndNotifyUser(success);
       
    }
    
    [RelayCommand]
    private void UpdateFilteredAchievements(string filter)
    {
        if (string.IsNullOrEmpty(filter))
        {
            AchievementsToDisplay = Achievements;
        }
        var filteredAchievements = Achievements.Where(x => x.DisplayName.Contains(filter, StringComparison.OrdinalIgnoreCase));
        AchievementsToDisplay = new ObservableCollection<GameAchievementViewModel>(filteredAchievements);
    }

    private async Task ReloadAndNotifyUser(bool success)
    {
        var toastManager = Ioc.Default.GetRequiredService<ISukiToastManager>();
        if (!success)
            toastManager.CreateToast()
                .OfType(NotificationType.Error)
                .WithTitle("Error").WithContent("Failed to save achievements.")
                .Dismiss().After(TimeSpan.FromSeconds(3))
                .Dismiss().ByClicking()
                .Queue();
        else
            toastManager.CreateToast()
                .OfType(NotificationType.Success)
                .WithTitle("Success").WithContent("Achievements saved.")
                .Dismiss().After(TimeSpan.FromSeconds(3))
                .Dismiss().ByClicking()
                .Queue();
        
        await LoadAsync();
    }
}