using Ebay_project.Models.DTOs;
using Ebay_project.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ebay_project.Controllers
{
    [Route("api")]
    [ApiController]
    public class PublicController : ControllerBase
    {
        private readonly IPublicService _publicService;

        public PublicController(IPublicService publicService)
        {
            _publicService = publicService;
        }


        [HttpPost("register")]
        public IActionResult Register(UserRegistrationDto userRegistration)
        {
            var response = _publicService.Register(userRegistration);

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
            var response = _publicService.Login(userLogin);

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

        [HttpGet("exception")]
        public IActionResult Exception()
        {
            var list = new List<int>();
            int a = list[1];
            return null;
        }

    }
}
