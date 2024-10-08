using System.Net;
using System.Net.WebSockets;
using System.Text;
namespace ConsoleChatApp.WS_Server;

public static class WebSocketHandler
{
    private static readonly List<WebSocket> _connections = [];

    public static void MapWebSocketHandler(this IEndpointRouteBuilder app, string path)
    {
        app.MapGet(path, async context =>
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var username = context.Request.Query["username"];
                using var ws = await context.WebSockets.AcceptWebSocketAsync();

                _connections.Add(ws);
                await BroadcastAsync($"{username} joined the room!");
                await BroadcastAsync($"{_connections.Count} users connected.");

                await ReceiveMessagesAsync(ws, async (result, buffer) =>
                {
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        await BroadcastAsync($"{username}: {message}");
                    }
                    else if (result.MessageType == WebSocketMessageType.Close || ws.State == WebSocketState.Aborted)
                    {
                        _connections.Remove(ws);
                        await BroadcastAsync($"{username} left the room");
                        await BroadcastAsync($"{_connections.Count} users connected");
                        await ws.CloseAsync(result.CloseStatus!.Value, result.CloseStatusDescription, CancellationToken.None);
                    }
                });
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        });
    }

    private static async Task BroadcastAsync(string message)
    {
        var bytes = Encoding.UTF8.GetBytes(message);
        var arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);

        foreach (var socket in _connections)
        {
            if (socket.State == WebSocketState.Open)
            {
                await socket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }

    private static async Task ReceiveMessagesAsync(WebSocket socket, Func<WebSocketReceiveResult, byte[], Task> handleMessage)
    {
        var buffer = new byte[1024 * 4];
        while (socket.State == WebSocketState.Open)
        {
            var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            await handleMessage(result, buffer);
        }
    }
}
