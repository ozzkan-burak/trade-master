namespace TradeMaster.Domain.Core;

public interface IEventStore
{
    void Save(Guid aggregateId, string aggregateType, IEnumerable<IDomainEvent> events, int expectedVersion);
    List<IDomainEvent> Get(Guid aggregateId);
}