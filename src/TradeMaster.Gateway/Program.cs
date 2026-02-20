using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

// CORS Ayarları (Öncekiyle aynı)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// YARP Ayarlarını Kod İçinde Tanımlayalım
var routes = new[]
{
    new RouteConfig { RouteId = "api-route", ClusterId = "stock-cluster", Match = new RouteMatch { Path = "/api/{**remainder}" } },
    new RouteConfig { RouteId = "signalr-route", ClusterId = "stock-cluster", Match = new RouteMatch { Path = "/hubs/{**remainder}" } }
};

var clusters = new[]
{
    new ClusterConfig
    {
        ClusterId = "stock-cluster",
        SessionAffinity = new SessionAffinityConfig
        {
            Enabled = true,
            Policy = "Cookie",
            AffinityKeyName = ".TradeMaster.StickyKey" // Hatanın çözümü tam olarak burası
        },
        Destinations = new Dictionary<string, DestinationConfig>
        {
            { "destination1", new DestinationConfig { Address = "http://localhost:5183" } }
        }
    }
};

builder.Services.AddReverseProxy().LoadFromMemory(routes, clusters);

var app = builder.Build();

app.UseCors("AllowReactApp");
app.MapReverseProxy();
app.Run();