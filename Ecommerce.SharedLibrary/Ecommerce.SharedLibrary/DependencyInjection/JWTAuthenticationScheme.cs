using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Ecommerce.SharedLibrary.DependencyInjection
{
    //JWT Authentication Scheme
    public static class JWTAuthenticationScheme
    {
        public static IServiceCollection AddJWTAuthenticationScheme(this IServiceCollection _service, IConfiguration _config)
        {
            //add jwt services
            _service.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer("Bearer", options =>
            {
                var key = Encoding.UTF8.GetBytes(_config.GetSection("Authentication:Key").Value!);
                string issuer = _config.GetSection("Authentication:Issuer").Value!;
                string audience = _config.GetSection("Authentication:Audience").Value!;


                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer=true,
                    ValidateAudience=true,
                    ValidateLifetime=false,//since we are not implementing refresh token hence false
                    ValidateIssuerSigningKey=true,

                    ValidIssuer=issuer,
                    ValidAudience=audience,
                    IssuerSigningKey=new SymmetricSecurityKey(key),
                };
            });
            return _service;
        }

    }
}
