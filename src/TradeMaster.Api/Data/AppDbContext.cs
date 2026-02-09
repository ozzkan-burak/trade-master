using Microsoft.EntityFrameworkCore;
using TradeMaster.Api.Models;

namespace TradeMaster.Api.Data;

public class AppDbContext : DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

  public DbSet<StockEvent> StockEvents { get; set; }
}