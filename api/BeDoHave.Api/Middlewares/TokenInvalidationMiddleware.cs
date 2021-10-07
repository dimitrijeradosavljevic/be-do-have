using BeDoHave.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BeDoHave.Middlewares
{
    public class TokenInvalidationMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenInvalidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, ITokenService tokenService)
        {
            if (httpContext.Request.Headers.TryGetValue("Authorization", out var token))
            {
                if (tokenService.InvalidateOrCheckAccessToken(token, true))
                {
                    await _next(httpContext);

                    return;
                }


                httpContext.Response.StatusCode = 401;
                httpContext.Response.ContentType = "application/json";

                await httpContext.Response.WriteAsJsonAsync(new { Code = 401, Message = "The access token is not valid." });
            }
            else
            {
                await _next(httpContext);
            }
        }
    }
}
