import { useEffect, useState } from 'react';
import * as signalR from '@microsoft/signalr';

// Gelen verinin tipini tanımlayalım (TypeScript avantajı)
interface StockUpdate {
  symbol: string;
  price: number;
  timestamp: string;
}

function App() {
  const [stocks, setStocks] = useState<StockUpdate[]>([]);
  const [connection, setConnection] = useState<signalR.HubConnection | null>(
    null,
  );

  useEffect(() => {
    // 1. Gateway üzerinden bağlantıyı yapılandır
    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5000/hubs/stocks') // Gateway Portu
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);
  }, []);

  useEffect(() => {
    if (connection) {
      connection
        .start()
        .then(() => {
          console.log('Sinyal kulesine bağlandık!');

          // 2. "ReceivePriceUpdate" mesajını dinle
          connection.on('ReceivePriceUpdate', (update: StockUpdate) => {
            setStocks((prev) => [update, ...prev].slice(0, 10)); // Son 10 güncellemeyi tut
          });
        })
        .catch((e) => console.log('Bağlantı hatası: ', e));
    }
  }, [connection]);

  return (
    <div style={{ padding: '20px', fontFamily: 'sans-serif' }}>
      <h1>TradeMaster Live Terminal</h1>
      <div
        style={{
          border: '1px solid #ccc',
          borderRadius: '8px',
          padding: '10px',
        }}
      >
        {stocks.length === 0 ? (
          <p>Veri bekleniyor...</p>
        ) : (
          <table style={{ width: '100%', textAlign: 'left' }}>
            <thead>
              <tr>
                <th>Sembol</th>
                <th>Fiyat</th>
                <th>Zaman</th>
              </tr>
            </thead>
            <tbody>
              {stocks.map((s, i) => (
                <tr key={i} style={{ borderBottom: '1px solid #eee' }}>
                  <td>
                    <strong>{s.symbol}</strong>
                  </td>
                  <td style={{ color: 'green' }}>{s.price.toFixed(2)} TL</td>
                  <td>{new Date(s.timestamp).toLocaleTimeString()}</td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>
    </div>
  );
}

export default App;
