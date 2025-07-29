using CommunityToolkit.Mvvm.ComponentModel;

namespace YASAM.ViewModels;

public class ViewModelBase : ObservableObject
{

}


public partial class PageViewModelBase : ViewModelBase
{
    [ObservableProperty]
    private string _displayName;
}



public partial class YourGamesViewModel : PageViewModelBase
{
    
}

public partial class IdlingGamesViewModel : PageViewModelBase
{
    
}