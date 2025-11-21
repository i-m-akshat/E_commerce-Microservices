using AuthenticationAPI.Infrastructure.DependencyInjection;
using Ecommerce.SharedLibrary.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureServices(builder.Configuration);
var app = builder.Build();
//to use custom policies of infrastructure layer 
app.UseInfrastructurePolicies();
// 2. Enable Swagger UI (index.html)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();  // This shows swagger/index.html
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
