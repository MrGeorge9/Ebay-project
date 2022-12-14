using Ebay_project.Models;
using Ebay_project.Models.DTOs;
using System.Security.Claims;

namespace Ebay_project.Services
{
    public interface IBuyerService
    {    
        string CreateItem(User user, NewItemDto newItem);
        List<ListAvailableDtoResponse> ListAvailableItems(ListAvailableDto listAvailable);
        ItemDetailsDto ItemDetails(int id);
        ItemDetailsDto BidOnItem(User user, int id, int bid);       
    }
}
