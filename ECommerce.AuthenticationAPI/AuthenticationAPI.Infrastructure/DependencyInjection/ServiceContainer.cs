using AuthenticationAPI.Application.Interfaces;
using AuthenticationAPI.Infrastructure.Data;
using AuthenticationAPI.Infrastructure.Respository;
using Ecommerce.SharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationAPI.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureServices (this IServiceCollection _service, IConfiguration _config)
        {
            //add database connectivity

            //add authentication scheme jwt

            SharedServiceContainer.AddSharedServices<AuthenticationDbContext>(_service, _config, _config["MySerilog:FileName"]!);
            // create Dependency injection 
            _service.AddScoped<IUser, UserRepo>();

            return _service;
        }

        public static IApplicationBuilder UseInfrastructurePolicies(this IApplicationBuilder app)
        {
            //Register middleware such as :
            //Global exception : Handle external errors
            //Listen only to api gateway : block all outsiders call 
            SharedServiceContainer.UseSharedPolicies(app);
            return app;
        }
    }
}
