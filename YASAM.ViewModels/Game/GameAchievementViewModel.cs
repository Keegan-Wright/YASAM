using CommunityToolkit.Mvvm.ComponentModel;

namespace YASAM.ViewModels;

public partial class GameAchievementViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool _achieved;
    
    [ObservableProperty]
    private bool _hidden;
    
    
    [ObservableProperty]
    private string _id;
    
    
    [ObservableProperty]
    private string _displayName;
    
    [ObservableProperty]
    private string _description;
    
    [ObservableProperty]
    private string _icon;

    [ObservableProperty]
    private string _icongray;

    [ObservableProperty]
    private bool _shouldToggle;

    [ObservableProperty] 
    private DateTimeOffset? _unlockTime;
    
    public string CheckboxMessage => Achieved ? "Lock" : "Unlock";
    
    public GameAchievementViewModel(string? achievementApiName, string achievementName, string achievementDescription, bool achievementAchieved, bool achievementHidden, string lockedIcon, string icon, DateTimeOffset? unlockTime)
    {
        _unlockTime = unlockTime;
        Id = achievementApiName ?? "Unknown Name";
        DisplayName =  achievementName;
        Description = achievementDescription;
        Achieved = achievementAchieved;
        Achieved = achievementAchieved;
        Hidden = achievementHidden;
        Icongray = lockedIcon;
        Icon = icon;
    }
}