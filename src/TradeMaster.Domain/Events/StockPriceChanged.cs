using TradeMaster.Domain.Core;

namespace TradeMaster.Domain.Events;

public record StockPriceChanged(
    Guid Id,
    decimal NewPrice,
    DateTime OccurredOn
) : IDomainEvent;