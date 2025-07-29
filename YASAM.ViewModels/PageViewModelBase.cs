using CommunityToolkit.Mvvm.ComponentModel;

namespace YASAM.ViewModels;

public partial class PageViewModelBase : ViewModelBase
{
    [ObservableProperty]
    private string _displayName;
}