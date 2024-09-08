using ConsoleChatApp.WS_Server;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:8080");

var app = builder.Build();
app.UseWebSockets();
app.MapWebSocketHandler("/ws");

await app.RunAsync();