using System.Net.WebSockets;
using System.Text;

namespace ConsoleChatApp.WS_Client;

public class WebSocketService
{
    public async Task SendMessagesAsync(ClientWebSocket ws)
    {
        while (true)
        {
            var message = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(message)) continue;
            if (message == "/exit") break;

            var bytes = Encoding.UTF8.GetBytes(message!);
            await ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }

    public async Task ReceiveMessagesAsync(ClientWebSocket ws)
    {
        var buffer = new byte[1024 * 4];
        while (true)
        {
            var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Close) break;

            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            Console.WriteLine($"{message}");
        }
    }
}
