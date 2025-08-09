using CommunityToolkit.Mvvm.ComponentModel;

namespace YASAM.ViewModels;

public partial class GameAchievementViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool _achieved;
    
    
    [ObservableProperty]
    private string _id;
    
    
    [ObservableProperty]
    private bool _displayName;
}