namespace ApiGateway.Presentation.Middlewares
{
    public class AttachSignatureToRequest(RequestDelegate _next) 
    {
        public async Task InvokeAsync(HttpContext _context)
        {
            _context.Request.Headers["Api-Gateway"] = "Signed";
            await _next(_context);
        }
    }
    
}
