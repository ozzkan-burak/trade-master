using TradeMaster.Domain.Core;
public interface IEventStore
{
  void Save(Guid aggregateId, IEnumerable<IDomainEvent> events, int expectedVersion);
  List<IDomainEvent> Get(Guid aggregateId);
}