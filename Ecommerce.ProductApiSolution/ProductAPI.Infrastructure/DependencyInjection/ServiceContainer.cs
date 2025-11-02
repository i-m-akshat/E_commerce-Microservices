using Ecommerce.SharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductAPI.Application.Interfaces;
using ProductAPI.Infrastructure.Data;
using ProductAPI.Infrastructure.Repositories;


namespace ProductAPI.Infrastructure.DependencyInjection
{
   public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection _services,IConfiguration _config)
        {
            //add database connectivity
            //add authentication scheme
            SharedServiceContainer.AddSharedServices<ProductDbContext>(_services, _config, _config["MySerilog:FineName"]!);
            //Create dependency injection(DI) 
            _services.AddScoped<IProduct, ProductRepo>();
            return _services;

        }
        public static IApplicationBuilder UseInfrastructurePolicies(this IApplicationBuilder _app)
        {
            //Register middleware such as :
            //Global Exception handler
            //Listen to only api gateway
            SharedServiceContainer.UseSharedPolicies(_app);
            return _app;
        }

    }
}
