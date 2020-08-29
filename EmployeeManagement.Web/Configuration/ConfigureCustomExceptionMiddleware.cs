using EmployeeManagement.Middleware;
using Microsoft.AspNetCore.Builder;

namespace EmployeeManagement.Configuration
{
    public static class ConfigureCustomExceptionMiddleware
    {
        public static void ConfigureExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
