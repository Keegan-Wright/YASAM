using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;

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
            Ioc.Default.GetRequiredService<YourGamesViewModel>(),
            Ioc.Default.GetRequiredService<IdlingGamesViewModel>(),
        ];
    }
}