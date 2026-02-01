using System.ComponentModel.DataAnnotations;

namespace TradeMaster.Infrastructure.Persistence.Models;

public class EventStoreModel
{
  [Key]
  public long Id { get; set; } // Olayların kesin sırasını takip etmek için
  public Guid AggregateId { get; set; }
  public string AggregateType { get; set; } = string.Empty;
  public int Version { get; set; }
  public string EventType { get; set; } = string.Empty;
  public string EventData { get; set; } = string.Empty; // JSON formatında IDomainEvent
  public DateTime OccurredOn { get; set; }
}