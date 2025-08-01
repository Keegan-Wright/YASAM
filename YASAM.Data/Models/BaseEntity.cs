namespace YASAM.Data.Models;

public class BaseEntity
{
    public Guid Id { get; private set; }
    public DateTimeOffset Created { get; init; }
    public DateTimeOffset Updated { get; private set; }
}