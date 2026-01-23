using TradeMaster.Domain.Core;
using TradeMaster.Domain.Events;

namespace TradeMaster.Domain.Entities;

public class Stock : AggregateRoot
{
  public string Symbol { get; private set; } = string.Empty;
  public decimal CurrentPrice { get; private set; }

  // AggregateRoot'un LoadFromHistory yapabilmesi için boş bir constructor şart
  public Stock() { }

  public Stock(Guid id, string symbol, string name, decimal initialPrice)
  {
    // Bir olay yarat ve ApplyChange ile hem listeye ekle hem de nesneyi güncelle
    ApplyChange(new StockCreated(id, symbol, name, initialPrice, DateTime.UtcNow));
  }

  public void UpdatePrice(decimal newPrice)
  {
    ApplyChange(new StockPriceChanged(Id, newPrice, DateTime.UtcNow));
  }

  // dynamic dispatch (ApplyChange) buradaki metodları bulup çalıştıracak
  public void Apply(StockCreated @event)
  {
    Id = @event.Id;
    Symbol = @event.Symbol;
    CurrentPrice = @event.InitialPrice;
  }

  public void Apply(StockPriceChanged @event)
  {
    CurrentPrice = @event.NewPrice;
  }
}