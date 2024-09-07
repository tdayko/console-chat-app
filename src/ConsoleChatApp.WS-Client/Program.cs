using System.Net.WebSockets;
using System.Text;

string name = string.Empty;
while (true)
{
    if (!string.IsNullOrWhiteSpace(name)) break;

    Console.Write("Input your name: ");
    name = Console.ReadLine()!.Trim();
}

var ws = new ClientWebSocket();
await ws.ConnectAsync(new Uri($"ws://localhost:6969/ws?name={name}"), CancellationToken.None);
Console.WriteLine("connected");

var sendTask = Task.Run(async () =>
{
    while (true)
    {
        var message = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(message)) continue;
        if (message == "/exit") break;

        var bytes = Encoding.UTF8.GetBytes(message!);
        await ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
    }
});

var receiveTask = Task.Run(async () =>
{
    var buffer = new byte[1024 * 4];
    while (true)
    {
        var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        if (result.MessageType == WebSocketMessageType.Close) break;

        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
        Console.WriteLine($"{message}");
    }
});

await Task.WhenAny(sendTask, receiveTask);

if (ws.State != WebSocketState.Closed)
{
    await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "closing", CancellationToken.None);
}

await Task.WhenAll(sendTask, receiveTask);