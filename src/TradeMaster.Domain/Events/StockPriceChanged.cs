using TradeMaster.Domain.Core;

namespace TradeMaster.Domain.Events;

public record StockPriceChanged(
    Guid AggregateId,
    decimal NewPrice
) : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}