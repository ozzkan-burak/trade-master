namespace TradeMaster.Domain.Core;

public abstract class AggregateRoot
{
  private readonly List<IDomainEvent> _uncommittedChanges = new();

  public Guid Id { get; protected set; }
  public int Version { get; protected set; } = -1;

  public IEnumerable<IDomainEvent> GetUncommittedChanges()
  {
    return _uncommittedChanges;
  }

  public void MarkChangesAsCommitted()
  {
    _uncommittedChanges.Clear();
    Version++;
  }

  // Event'i listeye ekler ve Apply metodunu çağırır (dynamic dispatch)
  protected void ApplyChange(IDomainEvent @event)
  {
    ApplyChange(@event, true);
  }

  private void ApplyChange(IDomainEvent @event, bool isNew)
  {
    // Reflection ile Apply metodunu bulup çağırıyoruz
    var applyMethod = GetType().GetMethod("Apply",
        System.Reflection.BindingFlags.Instance |
        System.Reflection.BindingFlags.NonPublic |
        System.Reflection.BindingFlags.Public,
        null,
        new[] { @event.GetType() },
        null);

    applyMethod?.Invoke(this, new object[] { @event });

    if (isNew)
    {
      _uncommittedChanges.Add(@event);
    }
  }

  // Event sourcing: Geçmiş eventlerden nesneyi yeniden oluştur
  public void LoadFromHistory(IEnumerable<IDomainEvent> history)
  {
    foreach (var @event in history)
    {
      ApplyChange(@event, false); // Listeye ekleme, sadece state güncelle
      Version++;
    }
  }
}