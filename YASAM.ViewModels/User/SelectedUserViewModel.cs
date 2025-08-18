using CommunityToolkit.Mvvm.ComponentModel;

namespace YASAM.ViewModels;

public partial class SelectedUserViewModel : ViewModelBase
{
    [ObservableProperty]
    private Guid? _id;
    
    [ObservableProperty]
    private string? _name;
    
    [ObservableProperty]
    private ulong? _steamUserId;

    [ObservableProperty]
    private string? _apiKey;

    public event EventHandler? SelectedUserUpdated;

    public void UpdateSelectedUser(Guid id, string name, ulong steamUserId, string apiKey)
    {
        Id = id;
        Name = name;
        SteamUserId = steamUserId;
        ApiKey = apiKey;
        
        SelectedUserUpdated?.Invoke(this, EventArgs.Empty);
    }
}