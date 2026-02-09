using Microsoft.AspNetCore.SignalR;
using TradeMaster.Api.Hubs;
using TradeMaster.Api.Data; // DbContext'in olduğu yer
using TradeMaster.Api.Models; // StockEvent modelinin olduğu yer

namespace TradeMaster.Api.Services;

public class StockService
{
  private readonly AppDbContext _context;
  private readonly IHubContext<StockHub> _hubContext;

  public StockService(AppDbContext context, IHubContext<StockHub> hubContext)
  {
    _context = context;
    _hubContext = hubContext;
  }

  public async Task SaveAndBroadcastEvent(string symbol, decimal price)
  {
    // 1. PostgreSQL'e Mühürleme (Event Sourcing)
    var stockEvent = new StockEvent
    {
      Symbol = symbol,
      Price = price,
      CreatedAt = DateTime.UtcNow
    };

    _context.StockEvents.Add(stockEvent);
    await _context.SaveChangesAsync();

    // 2. SignalR ile Tüm Dünyaya Duyurma
    // "ReceivePriceUpdate" frontend'in dinleyeceği anahtar kelime olacak
    await _hubContext.Clients.All.SendAsync("ReceivePriceUpdate", new
    {
      Symbol = symbol,
      Price = price,
      Timestamp = stockEvent.CreatedAt
    });
  }
}