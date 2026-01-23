using TradeMaster.Domain.Core;

namespace TradeMaster.Domain.Events;

public record StockCreated(
    Guid Id,
    string Symbol,
    string Name,
    decimal InitialPrice,
    DateTime OccurredOn
) : IDomainEvent;