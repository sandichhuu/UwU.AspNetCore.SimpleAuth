using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace UwU.AspNetCore.SimpleAuth
{
    public class SecretKeyAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly SecretKeyProvider _provider;

        public SecretKeyAuthMiddleware(RequestDelegate next, SecretKeyProvider provider)
        {
            _next = next;
            _provider = provider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Lấy secret từ query string:  ?secret=abcd
            var hasSecret = context.Request.Query.TryGetValue("secret", out var secretValue);
            var provided = hasSecret ? secretValue.ToString() : null;

            var isValid = !string.IsNullOrEmpty(provided) && _provider.Compare(System.Text.Encoding.UTF8.GetBytes(provided));

            if (isValid)
            {
                // Tạo một ClaimsPrincipal “định danh” tối thiểu
                var claims = new[] { new Claim(ClaimTypes.Name, "SignalRClient") };
                var identity = new ClaimsIdentity(claims, "SecretKey");
                context.User = new ClaimsPrincipal(identity);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid or missing secret");
                return;
            }

            await _next(context);
        }
    }
}