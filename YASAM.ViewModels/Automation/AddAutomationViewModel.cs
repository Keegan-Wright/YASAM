using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using YASAM.SteamInterface.Models.Internal;

namespace YASAM.ViewModels;

public partial class AddAutomationViewModel : ViewModelBase
{
    [ObservableProperty] private string? _name;
    [ObservableProperty] private string? _cronExpression;
    [ObservableProperty] private int? _retries;
    [ObservableProperty] private ObservableCollection<int>? _retryIntervals;
    [ObservableProperty] private ObservableCollection<GameToInvoke>? _AffectedApps;
    [ObservableProperty] private bool _loading;
    
    [ObservableProperty] private ObservableCollection<GameViewModel> _gamesToDisplay = [];
    [ObservableProperty] private ObservableCollection<GameViewModel> _ownedGames = [];

    
}