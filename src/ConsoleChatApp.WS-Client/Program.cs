using System.Net.WebSockets;
using ConsoleChatApp.WS_Client;

static string GetUserName()
{
    string name;
    do
    {
        Console.Write("Input your name: ");
        name = Console.ReadLine()!.Trim();
    } while (string.IsNullOrWhiteSpace(name));

    return name;
}

string name = GetUserName();
using var ws = new ClientWebSocket();

await ws.ConnectAsync(new Uri($"ws://localhost:8080/ws?name={name}"), CancellationToken.None);
Console.WriteLine("Connected");

var webSocketService = new WebSocketService();

var sendTask = Task.Run(() => webSocketService.SendMessagesAsync(ws));
var receiveTask = Task.Run(() => webSocketService.ReceiveMessagesAsync(ws));

await Task.WhenAny(sendTask, receiveTask);

if (ws.State != WebSocketState.Closed)
{
    await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
}

await Task.WhenAll(sendTask, receiveTask);