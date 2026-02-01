using TradeMaster.Domain.Core;
using TradeMaster.Domain.Events;

namespace TradeMaster.Domain.Entities;

public class Stock : AggregateRoot
{
  public string Symbol { get; private set; } = string.Empty;
  public string Name { get; private set; } = string.Empty;
  public decimal CurrentPrice { get; private set; }

  // Rehydration için boş constructor
  public Stock() { }

  public Stock(Guid id, string symbol, string name, decimal initialPrice)
  {
    // Bir olay yarat ve ApplyChange ile hem listeye ekle hem de nesneyi güncelle
    ApplyChange(new StockCreated(id, symbol, name, initialPrice));
  }

  public void UpdatePrice(decimal newPrice)
  {
    ApplyChange(new StockPriceChanged(Id, newPrice));
  }

  // --- EVENT HANDLERS (Apply metodları) ---
  // ApplyChange bu metodları dinamik olarak bulup çalıştırır

  private void Apply(StockCreated @event)
  {
    Id = @event.AggregateId;
    Symbol = @event.Symbol;
    Name = @event.Name;
    CurrentPrice = @event.Price;
  }

  private void Apply(StockPriceChanged @event)
  {
    CurrentPrice = @event.NewPrice;
  }
}