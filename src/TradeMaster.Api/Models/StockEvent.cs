namespace TradeMaster.Api.Models;

public class StockEvent
{
  public int Id { get; set; }
  public string Symbol { get; set; } = string.Empty;
  public decimal Price { get; set; }
  public DateTime CreatedAt { get; set; }
}