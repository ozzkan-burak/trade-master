using System.Text.Json;
using TradeMaster.Domain.Core;
using TradeMaster.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace TradeMaster.Infrastructure.Persistence;

public class EventStoreRepository : IEventStore
{
  private readonly TradeMasterDbContext _context;

  public EventStoreRepository(TradeMasterDbContext context)
  {
    _context = context;
  }

  // IEventStore interface'indeki Save metodunu implemente ediyoruz (3 parametre)
  public void Save(Guid aggregateId, IEnumerable<IDomainEvent> events, int expectedVersion)
  {
    foreach (var @event in events)
    {
      var eventModel = new EventStoreModel
      {
        AggregateId = aggregateId,
        AggregateType = @event.GetType().DeclaringType?.Name ?? "Unknown", // Event'ten aggregate type çıkar
        Version = ++expectedVersion,
        EventType = @event.GetType().Name,
        EventData = JsonSerializer.Serialize((object)@event), // Gerçek runtime tipini kullanır
        OccurredOn = @event.OccurredOn
      };

      _context.EventStore.Add(eventModel);
    }

    _context.SaveChanges();
  }

  // IEventStore interface'indeki Get metodunu implemente ediyoruz (List<IDomainEvent> döner)
  public List<IDomainEvent> Get(Guid aggregateId)
  {
    var eventModels = _context.EventStore
        .Where(e => e.AggregateId == aggregateId)
        .OrderBy(e => e.Version)
        .AsNoTracking()
        .ToList();

    var domainEvents = new List<IDomainEvent>();

    foreach (var model in eventModels)
    {
      // Olay tipini string isminden buluyoruz (Örn: "StockCreatedEvent")
      var type = AppDomain.CurrentDomain.GetAssemblies()
          .SelectMany(a => a.GetTypes())
          .FirstOrDefault(t => t.Name == model.EventType);

      if (type != null)
      {
        var @event = JsonSerializer.Deserialize(model.EventData, type) as IDomainEvent;
        if (@event != null)
        {
          domainEvents.Add(@event);
        }
      }
    }

    return domainEvents;
  }
}