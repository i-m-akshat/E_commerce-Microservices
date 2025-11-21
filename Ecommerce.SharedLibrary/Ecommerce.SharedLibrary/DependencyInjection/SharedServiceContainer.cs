

using Ecommerce.SharedLibrary.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Ecommerce.SharedLibrary.DependencyInjection
{
    public static class SharedServiceContainer
    {
        //generic method
        public static IServiceCollection AddSharedServices<TContext>(this IServiceCollection _services, IConfiguration _config, string fileName) where TContext:DbContext
        {
            //add Generic Db Context 
            _services.AddDbContext<TContext>(options => options.UseSqlServer(_config.GetConnectionString("eCommerceConnection"), sqlserverOption =>
            {
                sqlserverOption.EnableRetryOnFailure();
            })); 
            //configuring Serilog for logging
            Log.Logger=new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.File(path:$"{fileName}-.text"
                ,restrictedToMinimumLevel:Serilog.Events.LogEventLevel.Information
                ,outputTemplate:"{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {message:lj}{NewLine}{Exception}"
                ,rollingInterval:RollingInterval.Day)
                .CreateLogger();

            //Add JWT Authentication Scheme
           
            _services.AddJWTAuthenticationScheme(_config);
            return _services;
        }

        public static IApplicationBuilder UseSharedPolicies(this IApplicationBuilder _app)
        {
            //use global exception handler
            _app.UseMiddleware<GlobalExceptionHandler>();


            //Register middleware to block outsider calls or non gateway calls which are not from gateway
            //_app.UseMiddleware<ListenToOnlyApiGateway>();
            return _app;

        }
    }
}
