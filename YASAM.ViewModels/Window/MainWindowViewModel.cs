using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using SukiUI.Dialogs;
using SukiUI.Toasts;

namespace YASAM.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly SelectedUserViewModel _selectedUserViewModel;

    [ObservableProperty] private ViewModelBase _activePage;

    [ObservableProperty] private bool _hasSelectedUser;

    [ObservableProperty] private ObservableCollection<PageViewModelBase> _pageList;

    public MainWindowViewModel(SelectedUserViewModel selectedUser)
    {
        DialogManager = Ioc.Default.GetRequiredService<ISukiDialogManager>();
        ToastManager = Ioc.Default.GetRequiredService<ISukiToastManager>();

        _selectedUserViewModel = selectedUser;
        HasSelectedUser = _selectedUserViewModel.Id != null;

        PageList =
        [
            Ioc.Default.GetRequiredService<LandingViewModel>()
        ];

        _activePage = Ioc.Default.GetRequiredService<LandingViewModel>();
        _selectedUserViewModel.SelectedUserUpdated += SelectUser;
    }


    public ISukiDialogManager DialogManager { get; init; }
    public ISukiToastManager ToastManager { get; init; }

    private void SelectUser(object? sender, EventArgs e)
    {
        if (!HasSelectedUser)
            PageList =
            [
                Ioc.Default.GetRequiredService<LandingViewModel>(),
                Ioc.Default.GetRequiredService<YourGamesViewModel>(),
                Ioc.Default.GetRequiredService<IdlingGamesViewModel>()
            ];
        HasSelectedUser = true;
        ActivePage = Ioc.Default.GetRequiredService<YourGamesViewModel>();
    }
}