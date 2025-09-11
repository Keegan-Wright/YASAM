using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace YASAM.ViewModels;

public partial class AutomationViewModel : ViewModelBase
{
    [ObservableProperty] 
    private Guid _cronId;
    
    [ObservableProperty]
    private string? _description;
    
    [ObservableProperty]
    private string? _expression;
    
    [ObservableProperty] private ObservableCollection<GameIdleAutomationViewModel> _games = [];

    public AutomationViewModel(Guid id, string description, string expression)
    {
        CronId = id;
        Description = description;
        Expression = expression;
    }
}