using Microsoft.EntityFrameworkCore;
using TradeMaster.Domain.Core;
using TradeMaster.Domain.Entities;
using TradeMaster.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database Configuration
builder.Services.AddDbContext<TradeMasterDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IEventStore, EventStoreRepository>();

var app = builder.Build();

/// --- GERÇEK DB TEST BLOĞU ---
using (var scope = app.Services.CreateScope())
{
    var eventStore = scope.ServiceProvider.GetRequiredService<IEventStore>();

    // 1. ADIM: Az önce DB'ye kaydettiğin ID'yi buraya yapıştır
    var idFromDb = Guid.Parse("cc2379cd-d847-4d86-aad3-672668dbb14b");

    Console.WriteLine($"\n🔍 DB'den Geri Yükleme Testi Başlıyor (ID: {idFromDb})");

    // 2. ADIM: DB'den bu ID'ye ait tüm geçmiş olayları çekiyoruz
    var storedEvents = eventStore.Get(idFromDb);
    Console.WriteLine($"✅ DB'den {storedEvents.Count()} adet olay başarıyla çekildi.");

    // 3. ADIM: Boş bir Stock nesnesi oluşturup geçmişi üzerine "yüklüyoruz"
    var recoveredStock = new Stock();
    recoveredStock.LoadFromHistory(storedEvents);

    // 4. ADIM: Sonuçları kontrol edelim
    Console.WriteLine("------------------------------------------");
    Console.WriteLine($"📈 Hisse Sembolü: {recoveredStock.Symbol}");
    Console.WriteLine($"🏢 Şirket Adı  : {recoveredStock.Name}");
    Console.WriteLine($"💰 Güncel Fiyat : {recoveredStock.CurrentPrice} TL");
    Console.WriteLine("------------------------------------------");
    Console.WriteLine("🚀 Tebrikler Mimar! Sistem geçmişten başarıyla ayağa kalktı.");
}
// ----------------------------

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
