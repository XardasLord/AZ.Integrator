namespace AZ.Integrator.Integrations.Domain.Aggregates;

public class SoftDeleteInfo
{
    public bool IsDeleted { get; }
    public DateTimeOffset? DeletedAt { get; }
    public Guid? DeletedBy { get; }

    private SoftDeleteInfo(bool isDeleted, DateTimeOffset? deletedAt, Guid? deletedBy)
    {
        IsDeleted = isDeleted;
        DeletedAt = deletedAt;
        DeletedBy = deletedBy;
    }

    public static SoftDeleteInfo NotDeleted() => new(false, null, null);

    public static SoftDeleteInfo Deleted(DateTimeOffset deletedAt, Guid deletedBy) => 
        new(true, deletedAt, deletedBy);
}

