using Microsoft.AspNetCore.SignalR;

namespace TradeMaster.Api.Hubs;

// Bu sınıf, kendisine bağlanan tüm istemcilere (frontend) mesaj göndermemizi sağlar.
public class StockHub : Hub
{
  // Bir istemci bağlandığında çalışır
  public override async Task OnConnectedAsync()
  {
    await base.OnConnectedAsync();
    // İstersen burada bağlanan kullanıcıya "Hoş geldin" diyebilirsin
  }
}