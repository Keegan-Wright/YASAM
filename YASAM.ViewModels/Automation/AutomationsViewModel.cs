using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using YASAM.Services.Client;

namespace YASAM.ViewModels;

public sealed partial class AutomationsViewModel : PageViewModelBase
{
    private readonly IAutomationService _automationService;
    

    [ObservableProperty] private ObservableCollection<AutomationViewModel> _automations = [];

    [ObservableProperty] private bool _loading;

    
    public AutomationsViewModel(IAutomationService automationService)
    {
        _automationService = automationService;
        DisplayName = "Automation";
    }

    public override string DisplayName { get; init; }

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (Automations.Any())
            return;
        
        Loading = true;
        
        await foreach (var automation in _automationService.GetGameIdlingAutomationsAsync())
        {
            if (Automations.Any(x => x.CronId == automation.CronTickerId))
            {
                Automations.First(x => x.CronId == automation.CronTickerId).Games.Add(new GameIdleAutomationViewModel(automation.AppId, automation.GameName, automation.IdleTime));
            }
            else
            {
                var newVm = new AutomationViewModel(automation.CronTickerId, automation.CronTicker.Description,
                    automation.CronTicker.Expression);
                newVm.Games.Add(new GameIdleAutomationViewModel(automation.AppId, automation.GameName, automation.IdleTime));
                Automations.Add(newVm);
            }
        }
            
        Loading = false;
    }

    [RelayCommand]
    private async Task AddAutomationAsync()
    {
        // if (!string.IsNullOrEmpty(NewUserName) && NewUserSteamId.HasValue && !string.IsNullOrEmpty(NewUserSteamApiKey))
        // {
        //     var newUser = await _automationService.AddTrackedUserAsync(NewUserName, NewUserSteamId.Value, NewUserSteamApiKey);
        //
        //     TrackedUsers.Add(new TrackedUserViewModel
        //         { Id = newUser.Id, Name = newUser.Name, SteamUserId = newUser.SteamId, ApiKey = newUser.ApiKey });
        //
        //     NewUserName = string.Empty;
        //     NewUserSteamApiKey = string.Empty;
        //     NewUserSteamId = null;
        // }
    }
    
}