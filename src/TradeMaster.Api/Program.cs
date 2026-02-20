using Microsoft.EntityFrameworkCore;
using TradeMaster.Api.Data;
using TradeMaster.Api.Services;
using TradeMaster.Api.Hubs;

var builder = WebApplication.CreateBuilder(args);

// DbContext Kaydı (PostgreSQL bağlantı dizini appsettings.json'da olmalı)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSignalR();
builder.Services.AddScoped<StockService>();

var app = builder.Build();

app.MapHub<StockHub>("/hubs/stocks");

app.MapPost("/api/stocks/update", async (string symbol, decimal price, StockService stockService) =>
{
    // Servis içinde oluşturulan nesneyi geri dönmesini sağlayabiliriz
    await stockService.SaveAndBroadcastEvent(symbol, price);

    // Artık response boş olmayacak
    return Results.Ok(new
    {
        Status = "Success",
        Message = $"Event mühürlendi: {symbol} - {price} TL",
        Time = DateTime.UtcNow
    });
});

app.Run();