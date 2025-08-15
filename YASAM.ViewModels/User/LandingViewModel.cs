using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using YASAM.Services.Client;

namespace YASAM.ViewModels;

public sealed partial class LandingViewModel : PageViewModelBase
{
    
    private readonly IUserService _userService;
    
    [ObservableProperty]
    private ObservableCollection<TrackedUserViewModel> _trackedUsers = [];
    
    [ObservableProperty]
    private string _newUserName;
    
    [ObservableProperty]
    private ulong _newUserSteamId;
    
    [ObservableProperty]
    private string _newUserSteamApiKey;
    public override string DisplayName { get; init; }

    public LandingViewModel(IUserService userService)
    {
        _userService = userService;
        DisplayName = "Landing Page";
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (TrackedUsers.Any())
            return;
        
        await foreach (var user in _userService.GetTrackedUsersAsync())
        {
            TrackedUsers.Add(new TrackedUserViewModel()
            {
                Id = user.Id,
                Name = user.Name,
                SteamUserId = user.SteamId,
                ApiKey = user.ApiKey,
            });
        }
    }
    
    [RelayCommand]
    private async Task AddTrackedUserAsync()
    {
        var newUser = await _userService.AddTrackedUserAsync(NewUserName, NewUserSteamId, NewUserSteamApiKey);
        
        TrackedUsers.Add(new TrackedUserViewModel(){ Id = newUser.Id, Name = newUser.Name, SteamUserId = newUser.SteamId, ApiKey = newUser.ApiKey });
        
        NewUserName = string.Empty;
        NewUserSteamApiKey = string.Empty;
        NewUserSteamId = 0;
    }

    [RelayCommand]
    private void SelectUser(TrackedUserViewModel user)
    {
        var selectedUser = Ioc.Default.GetRequiredService<SelectedUserViewModel>();
        selectedUser.UpdateSelectedUser(user.Id, user.Name, user.SteamUserId, user.ApiKey);
        
    }

}