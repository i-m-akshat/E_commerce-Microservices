

using AuthenticationAPI.Application.DTOs;
using AuthenticationAPI.Application.Interfaces;
using AuthenticationAPI.Domain.Entities;
using AuthenticationAPI.Infrastructure.Data;
using BCrypt.Net;
using Ecommerce.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationAPI.Infrastructure.Respository
{
    public class UserRepo(AuthenticationDbContext _context, IConfiguration _config) : IUser
    {
        private async Task<AppUser> GetUserByEmail(string email)
        {
            var user=await _context.Users.FirstOrDefaultAsync(x=>x.Email==email);
            return user!=null?user:null!;
        }
        public async Task<GetUserDTO> GetUser(int userID)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u=>u.Id==userID);
            return user is not null ? new GetUserDTO(
                user.Id,
                user.Name!,
                user.Address!,
                user.TelephoneNumber!,
                user.Email!,user.Role!)
                 : null!;
        }

        public async Task<Response> Login(LoginDTO _loginDTO)
        {
            var user = await GetUserByEmail(_loginDTO.email);
            if(user is null)
            {
                return new Response(false, "Invalid Credentials");
            }
            bool verfiyPassword = BCrypt.Net.BCrypt.Verify(_loginDTO.password,user.Password);
            if (!verfiyPassword)
            {
                return new Response(false, "Invalid Credentials");
            }

            string token = GenerateToken(user);
            return new Response(true, token);
        }

        private string GenerateToken(AppUser user)
        {
            var key = Encoding.UTF8.GetBytes(_config.GetSection("Authentication:Key").Value);
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim> {
            new (ClaimTypes.Name,user.Name!),
            new (ClaimTypes.Email,user.Email!)
            //,new (ClaimTypes.Role,user.Role!)
            };
            if (!string.IsNullOrEmpty(user.Role) || !Equals("string", user.Role))
            {
                claims.Add(new(ClaimTypes.Role, user.Role!));
            }

            var authToken = new JwtSecurityToken
                (
                    issuer: _config["Authentication:Issuer"],
                    audience: _config["Authentication:Audience"],
                    claims: claims,
                    expires: null,
                    signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(authToken);
        }

        public async Task<Response> Register(AppUserDTO _appUserDTO)
        {
            var getUser = await GetUserByEmail(_appUserDTO.email);
            if (getUser is null) {
                var result = await _context.Users.AddAsync(new AppUser()
                {
                    Name=_appUserDTO.name,
                    Email=_appUserDTO.email,
                    Password=BCrypt.Net.BCrypt.HashPassword(_appUserDTO.password),
                    TelephoneNumber=_appUserDTO.telephoneNumber,
                    Address=_appUserDTO.addresss,
                    Role=_appUserDTO.role
                });
                await _context.SaveChangesAsync();
                return result.Entity.Id > 0 ? new Response(true, "User Registered Successfully"):new Response(false,"Invalid Data provided ");
            }
            else
            {
                return new Response(false, $"you cannot use this email for registration");
            }
        }
    }
}
