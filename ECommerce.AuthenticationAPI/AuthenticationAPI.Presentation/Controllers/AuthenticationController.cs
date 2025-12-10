using AuthenticationAPI.Application.DTOs;
using AuthenticationAPI.Application.Interfaces;
using Ecommerce.SharedLibrary.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController(IUser _userInterface) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<Response>> Register(AppUserDTO _appUser)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result=await _userInterface.Register(_appUser);

            return result.Flag?Ok(result) : BadRequest(Request);
        }
        [HttpPost("login")]
        public async Task<ActionResult<Response>> Login(LoginDTO _dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _userInterface.Login(_dto);
            return result.Flag ? Ok(result) : BadRequest(Request);
        }
        [HttpGet(@"{id:int}")]
        [Authorize]
        public async Task<ActionResult<GetUserDTO>> GetUser(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid User id");
            }
            var user = await _userInterface.GetUser(id);
            return user.id>0 ? Ok(user) : NotFound(Request);
        }
    }
}
