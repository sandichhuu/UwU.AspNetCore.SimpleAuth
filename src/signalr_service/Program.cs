using signalr_service.Hubs;
using System.Security.Cryptography;
using UwU.AspNetCore.SimpleAuth;

namespace signalr_service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSignalR();
            builder.AddSimpleAuth("HUB_SECRET_KEY", CryptographicOperations.FixedTimeEquals);

            var app = builder.Build();
            app.UseSimpleAuth(["chat"]);
            app.MapHub<ChatHub>("chat");
            app.Run();
        }
    }
}
