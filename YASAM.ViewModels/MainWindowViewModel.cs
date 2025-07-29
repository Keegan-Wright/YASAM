using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace YASAM.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    
    [ObservableProperty]
    private ViewModelBase _activePage;
    
    [ObservableProperty]
    private ObservableCollection<PageViewModelBase> _pageList;
    
    public MainWindowViewModel()
    {
        PageList =
        [
            new YourGamesViewModel() { DisplayName = "Your Games" },
            new IdlingGamesViewModel() { DisplayName = "Idling Games" },
        ];
    }
}