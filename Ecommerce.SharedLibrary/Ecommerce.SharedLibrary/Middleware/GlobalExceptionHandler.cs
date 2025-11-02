
using Ecommerce.SharedLibrary.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;


namespace Ecommerce.SharedLibrary.Middleware
{
    public class GlobalExceptionHandler(RequestDelegate _next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            //declare variables
            string message = "sorry , internal server errror occurred. Kindly Try again";
            int statusCode = (int)HttpStatusCode.InternalServerError;
            string Title = "Internal Server Error";
            try
            {
                //let the controller process 
                await _next(context);
                //post controller return the response check if the response contains any of the matching status code and then reorganize the problem details according to that status code basically we are trying to customize the error response from api 

                //check if exception here is too many request 429 

                if (context.Response.StatusCode == (int)HttpStatusCode.TooManyRequests)
                {
                    Title = "Warning !";
                    message = "Too Many Request have been made";
                    statusCode= (int)HttpStatusCode.TooManyRequests;
                    await ModifyResponseHeader(context, Title, message, statusCode);
                }

                // If Response is UnAuthorized//401 status code 
                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    Title = "Alert !";
                    message = "You are not authorized to accesss !";
                    statusCode= (int)HttpStatusCode.Unauthorized;
                    await ModifyResponseHeader(context, Title, message, statusCode);
                }


                //if Response if forbidden
                if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    Title = "Out of Access !";
                    message = "Your are not allowed/required to access";
                    statusCode = (int)HttpStatusCode.Forbidden;
                    await ModifyResponseHeader(context, Title, message, statusCode);
                }
            }
            catch (Exception ex)
            {
                //Log original Exception /Console/File
                LogException.LogExceptions(ex);
                
                //check if Exception is timeout
                if(ex is TaskCanceledException|| ex is TimeoutException)
                {
                    Title = "Out of time!";
                    message = "Request Timeout.... Try again.";
                    statusCode = (int)HttpStatusCode.RequestTimeout;
                }

                //if none of these exceptions || exceptions caught then do the default 
                await ModifyResponseHeader(context, Title, message, statusCode);
            }
        }
        private async Task ModifyResponseHeader(HttpContext context,string Title,string message,int statuscode)
        {
            //display scary-free messages to client 

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(
                JsonSerializer.Serialize
                (
                    new ProblemDetails()
                                        {
                                            Detail=message,
                                            Status=statuscode,
                                            Title=Title
                                        }
                 ),CancellationToken.None);
            //context.Response.StatusCode = statuscode;
            return;
           
        }
    }
}
