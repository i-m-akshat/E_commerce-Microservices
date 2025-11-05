using Ecommerce.SharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Application.Interfaces;
using OrderApi.Infrastructure.Data;
using OrderApi.Infrastructure.Repositories;

namespace OrderApi.Infrastructure.DependencyInjection
{
   public static class ServiceContainer
    {

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection _services, IConfiguration _config)
        {
            //Add Database connectivity
            //add authentication scheme
            SharedServiceContainer.AddSharedServices<OrderDbContext>(_services,_config, _config["MySerilog:FileName"]!) ;
            //create Dependency Injection 
            _services.AddScoped<IOrder, OrderRepo>();
            return _services;
        }
        public static IApplicationBuilder UseInfrastructurePolicies(this IApplicationBuilder _app)
        {
            //Register middleware such as :
            //Global Exception handler->handles external errors 
            //ListenToApiOnly->block all outside calls

            SharedServiceContainer.UseSharedPolicies(_app);
            return _app; 
        }
    }
}
