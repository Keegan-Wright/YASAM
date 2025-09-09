using TickerQ.EntityFrameworkCore.Entities;

namespace YASAM.Data.Models;

public class AutomaticIdlingConfiguration : BaseEntity
{
    
    public ulong AppId { get; set; }
    public string GameName { get; set; }
    public int IdleTime { get; set; }
    
    public Guid CronTickerId { get; set; }
    
    
    public virtual CronTickerEntity CronTicker { get; set; }
}