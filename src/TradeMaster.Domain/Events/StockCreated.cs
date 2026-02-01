using TradeMaster.Domain.Core;

namespace TradeMaster.Domain.Events;

public record StockCreated(
    Guid AggregateId,
    string Symbol,
    string Name,
    decimal Price
) : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}