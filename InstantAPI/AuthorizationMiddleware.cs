using InstantAPI.Helpers;
using System.IdentityModel.Tokens.Jwt;

namespace InstantAPI
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public AuthorizationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public Task Invoke(HttpContext httpContext)
        {
            string path = httpContext.Request.Path.ToString().Replace("/api/", "");

            string method = httpContext.Request.Method;
            var authorizedRoles = _configuration.GetSection($"Authorizations:{method}:{path}")?.GetChildren()?.Select(x => x.Value)?.ToList();
            var isRestricted = authorizedRoles?.Count > 0;
            if (isRestricted)
            {
                httpContext.Request.Headers.TryGetValue("Authorization", out var bearer);
                string token = bearer.ToString().Replace("Bearer ", "");
                if (SecurityHelper.ValidateJwt(token))
                {
                    var decoded = new JwtSecurityTokenHandler().ReadJwtToken(token);
                    var role = decoded.Claims.First(claim => claim.Type == "Role").Value;
                    if (authorizedRoles.Contains(role))
                    {
                        return _next(httpContext);
                    }
                }
                httpContext.Response.StatusCode = 401;
                return Task.CompletedTask;
            }



            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuthorizationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthorizationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthorizationMiddleware>();
        }
    }
}
