using Microsoft.EntityFrameworkCore;
using TradeMaster.Infrastructure.Persistence.Models;

namespace TradeMaster.Infrastructure.Persistence;

public class TradeMasterDbContext : DbContext
{
  public TradeMasterDbContext(DbContextOptions<TradeMasterDbContext> options)
    : base(options)
  {
  }

  public DbSet<EventStoreModel> EventStore { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<EventStoreModel>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.AggregateId).IsRequired();
      entity.Property(e => e.AggregateType).IsRequired().HasMaxLength(255);
      entity.Property(e => e.Version).IsRequired();
      entity.Property(e => e.EventType).IsRequired().HasMaxLength(255);
      entity.Property(e => e.EventData).IsRequired();
      entity.Property(e => e.OccurredOn).IsRequired();

      // Index'ler
      entity.HasIndex(e => new { e.AggregateId, e.Version }).IsUnique();
      entity.HasIndex(e => e.OccurredOn);
    });
  }
}
