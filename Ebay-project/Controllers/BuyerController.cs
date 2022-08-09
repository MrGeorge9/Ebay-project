using Ebay_project.Models.DTOs;
using Ebay_project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ebay_project.Controllers
{
    [Authorize(Roles = "Admin, Buyer")]
    [Route("api")]
    [ApiController]
    public class BuyerController : ControllerBase
    {
        private readonly IBuyerService _buyerService;
        private readonly IAuthService _authService;

        public BuyerController(IBuyerService buyerService, IAuthService authService)
        {
            _buyerService = buyerService;
            _authService = authService;
        }

        [HttpPost("item")]
        public IActionResult CreateItem(NewItemDto newItem)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userClaims = identity.Claims;
            var user = _authService.ReturnUserFromToken(userClaims);
            var response = _buyerService.CreateItem(user, newItem);

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
       
        [HttpPost("list")]
        public IActionResult ListAvailableItems(ListAvailableDto listAvailable)
        {            
            var response = _buyerService.ListAvailableItems(listAvailable);            

            switch (response[0].Name)
            {
                case "Selected page must be higher than 0":
                    return BadRequest(new ErrorDto(response[0].Name));
                case "This page is empty":
                    return Ok(new StatusDto(response[0].Name));              
            }          
            return Ok(response);
        }
      
        [HttpPost("details")]
        public IActionResult ItemDetails([FromHeader] int id)
        {
            var response = _buyerService.ItemDetails(id);

            switch (response.Name)
            {
                case "No such item in the database":
                    return BadRequest(new ErrorDto(response.Name));              
            }
            return Ok(response);
        }
      
        [HttpPost("bid")]
        public IActionResult BidOnItem([FromHeader] int id, [FromHeader] int bid)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userClaims = identity.Claims;
            var user = _authService.ReturnUserFromToken(userClaims);
            var response = _buyerService.BidOnItem(user, id, bid);

            switch (response.Name)
            {
                case "No such item in the database":
                    return BadRequest(new ErrorDto(response.Name));
                case "User doesn´t have any money :(":
                    return BadRequest(new ErrorDto(response.Name));
                case "This item is already sold":
                    return BadRequest(new ErrorDto(response.Name));
                case "User has less money that the bid he wants to place":
                    return BadRequest(new ErrorDto(response.Name));
                case "The bid is lower than the last bid":
                    return BadRequest(new ErrorDto(response.Name));             
            }
            return Ok(response);
        }       
    }
}
