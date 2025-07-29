namespace YASAM.ViewModels;

public partial class IdlingGamesViewModel : PageViewModelBase
{
    public override string DisplayName { get; init; }

    public IdlingGamesViewModel()
    {
        DisplayName = "Idling Games";
    }
}