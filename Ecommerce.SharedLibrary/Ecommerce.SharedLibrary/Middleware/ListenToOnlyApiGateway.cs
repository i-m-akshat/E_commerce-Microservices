using Azure.Core;
using Microsoft.AspNetCore.Http;


namespace Ecommerce.SharedLibrary.Middleware
{
    public class ListenToOnlyApiGateway(RequestDelegate _next)
    {

        public async Task InvokeAsync(HttpContext context)
        {
            //Extract  specific header which will be added from the client request 
            var signedHeader = context.Request.Headers["Api-Gateway"];

            //NULL means the request is not coming from api gateway then service unavailable 503 
            if (signedHeader.FirstOrDefault() is null)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsync("Sorry, service is unavailable!");
                return;
            }
            else
            {
                await _next(context);
            }
        }
    }
}
