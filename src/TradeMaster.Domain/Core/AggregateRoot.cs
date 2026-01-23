namespace TradeMaster.Domain.Core;

public abstract class AggregateRoot
{
  public Guid Id { get; protected set; }
  public int Version { get; protected set; } = -1; // Versiyon takibi (Concurrency için)

  // Henüz veritabanına kaydedilmemiş yeni olaylar
  private readonly List<IDomainEvent> _changes = new();

  public IReadOnlyCollection<IDomainEvent> GetUncommittedChanges() => _changes.AsReadOnly();

  public void MarkChangesAsCommitted()
  {
    _changes.Clear();
  }

  // Geçmişten gelen olayları yükle ve nesneyi yeniden oluştur (Replay)
  public void LoadFromHistory(IEnumerable<IDomainEvent> history)
  {
    foreach (var e in history)
    {
      ApplyChange(e, isNew: false);
    }
  }

  protected void ApplyChange(IDomainEvent @event, bool isNew = true)
  {
    // C# dynamic kullanarak ilgili 'Apply' metodunu bul ve çalıştır
    ((dynamic)this).Apply((dynamic)@event);

    if (isNew)
    {
      _changes.Add(@event);
    }
  }
}