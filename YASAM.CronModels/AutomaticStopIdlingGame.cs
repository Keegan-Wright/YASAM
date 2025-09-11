namespace YASAM.CronModels;

public class AutomaticStopIdlingGame
{
    public AutomaticStopIdlingGame(Guid cronTickerId, ulong appId)
    {
        CronTickerId = cronTickerId;
        AppId = appId;
    }

    public Guid CronTickerId { get; set; }
    public ulong AppId { get; set; }
}