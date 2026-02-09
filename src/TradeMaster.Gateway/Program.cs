var builder = WebApplication.CreateBuilder(args);

// YARP servislerini ekle ve yapılandırmayı appsettings'ten oku
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();
app.MapGet("/api/stocks/hello", () => "Merhaba! Gateway üzerinden bana ulaştın.");

// Proxy rotalarını aktif et
app.MapReverseProxy();
app.Run();
