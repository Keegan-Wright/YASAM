using CommunityToolkit.Mvvm.ComponentModel;

namespace YASAM.ViewModels;

public partial class TrackedUserViewModel : ViewModelBase
{
    [ObservableProperty]
    private string? _name;
    
    [ObservableProperty]
    private ulong? _steamUserId;
    
    [ObservableProperty]
    private string? _apiKey;

    [ObservableProperty]
    private Guid? _id;
}