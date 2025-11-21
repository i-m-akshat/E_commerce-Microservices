using AuthenticationAPI.Application.DTOs;
using Ecommerce.SharedLibrary.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationAPI.Application.Interfaces
{
    public interface IUser
    {
        Task<Response> Register(AppUserDTO _appUserDTO);
        Task<Response> Login(LoginDTO _loginDTO);
        Task<GetUserDTO> GetUser(int userID);
    }
}
