# TradeMaster: Advanced Stock Trading & Orchestration Lab

TradeMaster, yüksek frekanslı hisse senedi piyasası verilerini yönetmek, dağıtık sistemlerde veri tutarlılığını sağlamak ve modern mimari desenleri deneyimlemek için kurgulanmış bir **Software Architect Lab** projesidir.

## Mimari Yaklaşım (Architectural Patterns)

- **Clean Architecture:** Bağımlılıkların içe doğru (Domain katmanına) olduğu, teknoloji değişimlerine karşı dirençli yapı.
- **Event Sourcing:** Nesnelerin durumu sadece anlık veri olarak değil, başlarından geçen tüm olayların (`IDomainEvent`) bir serisi olarak tutulur.
- **Domain-Driven Design (DDD):** İş kuralları `AggregateRoot` yapıları etrafında toplanarak karmaşıklık kapsüllenir.
- **CQRS:** Yazma (Command) ve okuma (Query) operasyonları performans için birbirinden ayrılmıştır.

## Teknoloji Yığını

- **Backend:** .NET 8, SignalR (Real-time data streaming).
- **Caching & Concurrency:** Redis (Distributed Lock ile Race Condition yönetimi).
- **Infrastructure:** Docker & Kubernetes (Orkestrasyon).
- **Frontend:** React 19, TypeScript, Tailwind CSS.

## Proje Yapısı

- **TradeMaster.Domain**: İş kuralları, Entity'ler ve Aggregate Root'lar.
- **TradeMaster.Application**: Use-case senaryoları ve DTO'lar.
- **TradeMaster.Infrastructure**: Veri tabanı ve Redis entegrasyonları.
- **TradeMaster.Api**: API Endpoint'leri ve WebSocket Hub'ları.

## Çözülen Temel Problemler

1. **Race Condition Management:** Eşzamanlı emirlerin tutarlı işlenmesi.
2. **High-Frequency Data Delivery:** Optimize edilmiş gerçek zamanlı veri akışı.
3. **Distributed Consistency:** Mikroservisler arası veri bütünlüğü.

"Bu proje, karmaşık mimari desenlerin pratik uygulamalarını test etmek ve modern yazılım standartlarını 'hand-on' deneyime dönüştürmek amacıyla geliştirilen açık kaynaklı laboratuvar çalışmasıdır."
