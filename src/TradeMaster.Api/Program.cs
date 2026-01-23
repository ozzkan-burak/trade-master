var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- TEST BLOĞU BAŞLANGICI ---
Console.WriteLine("!!! TERMINAL TEST: BURAYI GORMELISIN !!!");
using (var scope = app.Services.CreateScope())
{
    Console.WriteLine("🚀 TradeMaster Mimari Testi Başlıyor...");

    var stockId = Guid.NewGuid();

    // 1. Yeni bir Hisse Senedi oluştur (StockCreated event'i tetiklenir)
    var stock = new TradeMaster.Domain.Entities.Stock(
        stockId,
        "THYAO",
        "Türk Hava Yolları",
        250.50m
    );

    // 2. Fiyat güncellemeleri yap (StockPriceChanged event'leri tetiklenir)
    stock.UpdatePrice(255.75m);
    stock.UpdatePrice(260.10m);

    // 3. Değişiklikleri (Olayları) kontrol et
    var changes = stock.GetUncommittedChanges();
    Console.WriteLine($"✅ Kaydedilmeyi bekleyen olay sayısı: {changes.Count()}");

    // 4. REPLAY TESTİ: Sıfır bir nesneye bu olayları yükle
    var reloadedStock = new TradeMaster.Domain.Entities.Stock();
    reloadedStock.LoadFromHistory(changes);

    Console.WriteLine($"🔍 Replay Sonucu:");
    Console.WriteLine($"   Hisse: {reloadedStock.Symbol}");
    Console.WriteLine($"   Son Fiyat: {reloadedStock.CurrentPrice} TL");
}
// --- TEST BLOĞU BİTİŞİ ---

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
