
using Microsoft.AspNetCore.Builder;
using System.Collections.Generic;

namespace Base.Middleware.Extensions
{
    public static class JustGatewayMiddlewareExtensions
    {
        public static IApplicationBuilder UseJustGatewayMiddleware(this IApplicationBuilder app, HeaderRequirement HeaderRequirementInstance)
        {
            return app.UseMiddleware<JustGatewayMiddleware>(HeaderRequirementInstance);
        }
    }

    public static class GetUserMiddlewareExtensions
    {
        public static IApplicationBuilder UseGetUserMiddleware(this IApplicationBuilder app, List<string> ExceptPaths)
        {
            return app.UseMiddleware<GetUserMiddleware>(ExceptPaths);
        }
    }

    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }
    }

    public static class CheckTokenMiddlewareExtensions
    {
        public static IApplicationBuilder UseCheckTokenMiddleware(this IApplicationBuilder app, List<string> ExceptPaths, string RedirectOnErrorPath)
        {
            return app.UseMiddleware<CheckTokenMiddleware>(ExceptPaths, RedirectOnErrorPath);
        }
    }
}
