using System.Net.WebSockets;
using ConsoleChatApp.WS_Client;

static string GetUserusername()
{
    string? username;
    do
    {
        Console.Write("input your username: ");
        username = Console.ReadLine();
    } while (string.IsNullOrWhiteSpace(username));

    return username.Trim();
}

Console.Clear();
string username = GetUserusername();
using var ws = new ClientWebSocket();

await ws.ConnectAsync(new Uri($"ws://localhost:8080/ws?username={username}"), CancellationToken.None);
Console.WriteLine("connected");

var sendTask = Task.Run(() => WebSocketService.SendMessagesAsync(ws, username));
var receiveTask = Task.Run(() => WebSocketService.ReceiveMessagesAsync(ws));

await Task.WhenAny(sendTask, receiveTask);

if (ws.State == WebSocketState.Closed)
{
    await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "closing", CancellationToken.None);
}

await Task.WhenAny(sendTask, receiveTask);