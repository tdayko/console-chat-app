using System.Net.WebSockets;
using ConsoleChatApp.WS_Client;

static string GetUserName()
{
    string? name;
    do
    {
        Console.Write("input your name: ");
        name = Console.ReadLine();
    } while (string.IsNullOrWhiteSpace(name));

    return name.Trim();
}

string name = GetUserName();
using var ws = new ClientWebSocket();

await ws.ConnectAsync(new Uri($"ws://localhost:8080/ws?name={name}"), CancellationToken.None);
Console.WriteLine("connected");

var sendTask = Task.Run(() => WebSocketService.SendMessagesAsync(ws));
var receiveTask = Task.Run(() => WebSocketService.ReceiveMessagesAsync(ws));

await Task.WhenAny(sendTask, receiveTask);

if (ws.State != WebSocketState.Closed)
{
    await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "closing", CancellationToken.None);
}

await Task.WhenAll(sendTask, receiveTask);