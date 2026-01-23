namespace TradeMaster.Domain.Core;

public interface IDomainEvent
{
  DateTime OccurredOn { get; }
}

