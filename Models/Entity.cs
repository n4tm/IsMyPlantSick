namespace IsMyPlantSickApp.Models;

/// <summary>Generic Entity</summary>
/// <typeparam name="T"></typeparam>
public abstract class Entity<TId> {
    public required TId Id { get; set; }

    public DateTime CreationTime { get; set; } = DateTime.UtcNow;

    public DateTime LastUpdateTime { get; set; } = DateTime.UtcNow;

    public virtual bool IsValid() => true;
}

public abstract class EntityFilter<TId, TEntity> 
    where TEntity : Entity<TId>
    where TId : IEquatable<TId> {

    public TId? Id { get; set; }

    public DateTime? CreationTime { get; set; }

    public DateTime? LastUpdateTime { get; set; }

    public IEnumerable<TEntity> Apply(IEnumerable<TEntity> entities) =>
        entities.Where(entity =>
            DefaultIsEqual(Id, entity.Id) &&
            DefaultIsEqual(CreationTime, entity.CreationTime) &&
            DefaultIsEqual(LastUpdateTime, entity.LastUpdateTime) &&
            FilterMatches(entity)
        );

    protected abstract bool FilterMatches(TEntity entity);

    protected static bool DefaultIsEqual<T>(T? p1, T? p2) =>
        p1 == null || p2 == null || p1.Equals(p2);

    protected static bool DefaultContains<T>(IEnumerable<T>? p1, T? p2) =>
        p1 == null || p2 == null || p1.Contains(p2);
}