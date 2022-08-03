using Ebay_project.Models.DTOs;
using Ebay_project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ebay_project.Controllers
{
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public IActionResult Register(UserRegistrationDto userRegistration)
        {
            var response = _userService.Register(userRegistration);

            switch (response)
            {
                case "No data provided":
                    return BadRequest(new ErrorDto(response));
                case "No name provided":
                    return BadRequest(new ErrorDto(response));
                case "No pasword provided":
                    return BadRequest(new ErrorDto(response));
                case "Username already taken":
                    return BadRequest(new ErrorDto(response));
                case "Password must be at least 8 characters long":
                    return BadRequest(new ErrorDto(response));
                case "Password must contain at leat one special character":
                    return BadRequest(new ErrorDto(response));               
            }
            return Ok(new StatusDto(response));
        }

        [HttpPost("login")]
        public IActionResult Login(UserLoginDto userLogin)
        {
            var response = _userService.Login(userLogin);

            switch (response)
            {
                case "No data provided":
                    return BadRequest(new ErrorDto(response));
                case "No name provided":
                    return BadRequest(new ErrorDto(response));
                case "No pasword provided":
                    return BadRequest(new ErrorDto(response));
                case "No such user":
                    return BadRequest(new ErrorDto(response));
                case "Password is incorrect":
                    return BadRequest(new ErrorDto(response));                
            }
            return Ok(new LoginResponseDto(response.Split("&")[0], Int32.Parse(response.Split("&")[1])));
        }
        
        [Authorize(Roles = "Admin, Buyer")]
        [HttpPost("createItem")]
        public IActionResult createItem(NewItemDto newItem)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userClaims = identity.Claims;
            var user = _userService.ReadUser(userClaims);
            var response = _userService.CreateItem(user, newItem);

            switch (response)
            {
                case "No data provided":
                    return BadRequest(new ErrorDto(response));
                case "No name provided":
                    return BadRequest(new ErrorDto(response));
                case "No description provided":
                    return BadRequest(new ErrorDto(response));
                case "No photoURL provided":
                    return BadRequest(new ErrorDto(response));
                case "Starting price can´t be less than 1":
                    return BadRequest(new ErrorDto(response));
                case "Purchase price can´t be less than 1":
                    return BadRequest(new ErrorDto(response));
                case "Photo URL is incorrect":
                    return BadRequest(new ErrorDto(response));                
            }
            return Ok(new ItemCreationResponseDto(response, newItem));
        }
    }
}
