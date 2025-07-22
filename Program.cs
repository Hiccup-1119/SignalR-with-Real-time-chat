using SignalRChat.Hubs;
using Grpc.Net.Client;
using GrpcServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.MapGrpcService<GreeterService>();

app.MapGet("/", () => "gRPC server is running. Use a gRPC client to talk to me.");

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.MapHub<StronglyTypedChatHub>("/Chat");

var serverTask = app.RunAsync();
await Task.Delay(1_000);

using var channel = GrpcChannel.ForAddress("https://localhost:5252");
var client = new Greeter.GreeterClient(channel);
var reply = await client.SayHelloAsync(
    new HelloRequest { Name = "GreeterClient" }
);
Console.WriteLine("Greeting:" + reply.Message);
Console.WriteLine("Press any key to exit...");
Console.ReadKey();

await serverTask;

app.Run();
