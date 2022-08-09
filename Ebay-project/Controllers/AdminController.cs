using Ebay_project.Models.DTOs;
using Ebay_project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ebay_project.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("wallet")]
        public IActionResult FillWalletOfUser([FromHeader] int userId, [FromHeader] int ammount)
        {
            var response = _adminService.FillWalletOfUser(userId, ammount);

            switch (response)
            {
                case "Ammount has to be higher than 0":
                    return BadRequest(new ErrorDto(response));
                case "No such user":
                    return BadRequest(new ErrorDto(response));
            }
            return Ok(new StatusDto(response));
        }

        [HttpDelete("user")]
        public IActionResult DeleteUser([FromHeader] int userId)
        {
            var response = _adminService.DeleteUser(userId);

            switch (response)
            {
                case "No such user":
                    return BadRequest(new ErrorDto(response));               
            }
            return Ok(new StatusDto(response));
        }
    }
}
