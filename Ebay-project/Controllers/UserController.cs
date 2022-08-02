using Ebay_project.Models.DTOs;
using Ebay_project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ebay_project.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(UserLogin userLogin)
        {
            var response = _userService.Login(userLogin);

            if (response == string.Empty)
            {
                return NotFound("User not found");
            }
            return Ok(response);
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPost("sell")]
        public IActionResult Login()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userClaims = identity.Claims;

            var response = _userService.ReadUser(userClaims);

            if (response == null)
            {
                return NotFound("User not found");
            }
            return Ok(response);
        }
    }
}
