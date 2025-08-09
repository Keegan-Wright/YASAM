using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using SukiUI.Dialogs;

namespace YASAM.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly SelectedUserViewModel _selectedUserViewModel;
    
    [ObservableProperty]
    private ViewModelBase _activePage;
    
    [ObservableProperty]
    private ObservableCollection<PageViewModelBase> _pageList;

    [ObservableProperty]
    private bool _hasSelectedUser;
    
    
    public ISukiDialogManager DialogManager { get; init; }
    
    public MainWindowViewModel(SelectedUserViewModel selectedUser)
    {
        DialogManager = Ioc.Default.GetRequiredService<ISukiDialogManager>();
        
        _selectedUserViewModel = selectedUser;
        HasSelectedUser = _selectedUserViewModel.Id != Guid.Empty;

        PageList =
        [
            Ioc.Default.GetRequiredService<LandingViewModel>(),
        ];

        _selectedUserViewModel.SelectedUserUpdated += SelectUser;
    }

    private void SelectUser(object? sender, EventArgs e)
    {
        if (!HasSelectedUser)
        {
            PageList =
            [
                Ioc.Default.GetRequiredService<LandingViewModel>(),
                Ioc.Default.GetRequiredService<YourGamesViewModel>(),
                Ioc.Default.GetRequiredService<IdlingGamesViewModel>(),
            ];
        }
        HasSelectedUser = true;
        

    }
}