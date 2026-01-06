using Microsoft.AspNetCore.SignalR.Client;

namespace signalr_client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            const string secret = "1234";
            var hubUrl = $"http://localhost:5266/chat?secret={Uri.EscapeDataString(secret)}";

            var connection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .Build();

            connection.On<string, string>("ReceiveMessage", (user, msg) =>
                Console.WriteLine($"[{user}] {msg}"));

            try
            {
                await connection.StartAsync();
                Console.WriteLine("🚀 Connected");
                await connection.InvokeAsync("SendMessage", "client1", "Hello from .NET client");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Connection failed: {ex.Message}");
            }

            Console.ReadLine();
            await connection.StopAsync();
        }
    }
}
