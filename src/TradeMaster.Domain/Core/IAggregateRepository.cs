namespace TradeMaster.Domain.Core;

public interface IAggregateRepository<T> where T : AggregateRoot
{
    void Save(T aggregate);
    T GetById(Guid id);
}