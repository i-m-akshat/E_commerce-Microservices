using Ecommerce.SharedLibrary.Logs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Application.Services;
using Polly;
using Polly.Retry;

namespace OrderApi.Application.DependencyInjection
{
    public static class Service_Container
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection _services,IConfiguration _config) 
        {
            //Register http client service 
            //Create dependency injection 

            _services.AddHttpClient<IOrderService, OrderService>(options =>
            {
                options.BaseAddress = new Uri(_config["ApiGateway:BaseAddress"]!);
                options.Timeout = TimeSpan.FromSeconds(1);
               
            });
            //Create Retry strategy resiliance

            var retryStrategy = new RetryStrategyOptions()
           {
              ShouldHandle= new PredicateBuilder().Handle<TaskCanceledException>(),
              BackoffType=DelayBackoffType.Constant,
              UseJitter=true,
              MaxRetryAttempts=3,
              Delay=TimeSpan.FromMilliseconds(500),
                OnRetry = args =>
                {
                    string message = $"OnRetry, Attempt{args.AttemptNumber} Outcome {args.Outcome}";
                    LogException.LogToConsole(message);
                    LogException.LogToDebugger(message);
                   return ValueTask.CompletedTask;
                }
           };
            //use retry strategy
            _services.AddResiliencePipeline("my-retry-pipeline", options =>
            {
                options.AddRetry(retryStrategy);
            });
            return _services;
        }

    }
}
