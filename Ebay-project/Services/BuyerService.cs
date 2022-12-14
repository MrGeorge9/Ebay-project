using Ebay_project.Context;
using Ebay_project.Models;
using Ebay_project.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace Ebay_project.Services
{
    public class BuyerService : IBuyerService
    {
        private readonly ApplicationContext _db;       

        public BuyerService(ApplicationContext db)
        {
            _db = db;
        }           

        public string CreateItem(User user, NewItemDto newItem)
        {
            if (newItem.Name == string.Empty && newItem.Description == string.Empty && newItem.PhotoUrl == string.Empty
                && newItem.StartingPrice == 0 && newItem.PurchasePrice == 0)
            {
                return "No data provided";
            }
            if (newItem.Name == string.Empty)
            {
                return "No name provided";
            }
            if (newItem.Description == string.Empty)
            {
                return "No description provided";
            }
            if (newItem.PhotoUrl == string.Empty)
            {
                return "No photoURL provided";
            }
            if (newItem.StartingPrice <= 0)
            {
                return "Starting price can´t be less than 1";
            }
            if (newItem.PurchasePrice <= 0)
            {
                return "Purchase price can´t be less than 1";
            }
            if (!(newItem.PhotoUrl.EndsWith("jpg") || newItem.PhotoUrl.EndsWith("jpeg") || newItem.PhotoUrl.EndsWith("png")))
            {
                return "Photo URL is incorrect";
            }

            Item item = new Item()
            {
                Name = newItem.Name,
                Description = newItem.Description,
                PhotoURL = newItem.PhotoUrl,
                StartPrice = newItem.StartingPrice,
                PurchasePrice = newItem.PurchasePrice
            };

            _db.Items.Add(item);
            user.Items.Add(item);
            _db.SaveChanges();

            return "Item is created";
        }

        public List<ListAvailableDtoResponse> ListAvailableItems(ListAvailableDto listAvailable)
        {
            List<Item> availableItems = new List<Item>();
            List<ListAvailableDtoResponse> listAvailablesResponses = new List<ListAvailableDtoResponse>();

            if (listAvailable.Page <= 0)
            {
                listAvailablesResponses.Add(new ListAvailableDtoResponse("Selected page must be higher than 0"));
                return listAvailablesResponses;
            }

            var itemPosition = (listAvailable.Page - 1) * 20;
            var items = _db.Items.Include(p => p.Bids).Where(p => p.Sold == false).ToList();

            if (items.Count > itemPosition + 20)
            {
                for (int i = itemPosition; i < itemPosition + 20; i++)
                {
                    availableItems.Add(items[i]);
                }
            }
            else if (items.Count > itemPosition)
            {
                for (int i = itemPosition; i < items.Count(); i++)
                {
                    availableItems.Add(items[i]);
                }
            }

            if (availableItems.Count == 0)
            {
                listAvailablesResponses.Add(new ListAvailableDtoResponse("This page is empty"));
                return listAvailablesResponses;
            }

            for (int i = 0; i < availableItems.Count(); i++)
            {
                listAvailablesResponses.Add(new ListAvailableDtoResponse(
                    availableItems[i].Name,
                    availableItems[i].PhotoURL,
                    availableItems[i].Bids.Count() == 0 ? 0 : availableItems[i].Bids.Last().Value));
            }

            return listAvailablesResponses;
        }

        public ItemDetailsDto ItemDetails(int id)
        {
            List<BidDto> bidDtos = new List<BidDto>();
            var item = _db.Items.Include(p => p.Bids).Include(p => p.User).FirstOrDefault(p => p.Id == id);
            if (item == null)
            {
                return new ItemDetailsDto("No such item in the database");
            }

            for (int i = 0; i < item.Bids.Count(); i++)
            {
                var userId = item.Bids[i].UserId;
                bidDtos.Add(new BidDto(item.Bids[i].Value, _db.Users.FirstOrDefault(p => p.Id == userId).Name));
            }
            ItemDetailsDto itemDetailsDto = new ItemDetailsDto(
                item.Name,
                item.Description,
                item.PhotoURL,
                bidDtos,
                item.PurchasePrice,
                item.User.Name,
                item.BuyersName
                );

            return itemDetailsDto;
        }

        public ItemDetailsDto BidOnItem(User user, int id, int bid)
        {
            List<BidDto> bidDtos = new List<BidDto>();
            var item = _db.Items.Include(p => p.Bids).FirstOrDefault(p => p.Id == id);

            if (item == null)
            {
                return new ItemDetailsDto("No such item in the database");
            }

            if (user.Wallet <= 0)
            {
                return new ItemDetailsDto("User doesn´t have any money :(");
            }


            if (item.Sold == true)
            {
                return new ItemDetailsDto("This item is already sold");
            }

            if (user.Wallet < bid)
            {
                return new ItemDetailsDto("User has less money that the bid he wants to place");
            }

            if (item.Bids.Count() > 0)
            {
                if (bid < item.Bids.Last().Value)
                {
                    return new ItemDetailsDto("The bid is lower than the last bid");
                }
            }

            if (bid < item.PurchasePrice)
            {
                item.Bids.Add(new Bid() { Item = item, User = user, Value = bid });
                _db.SaveChanges();
            }
            else
            {
                item.Bids.Add(new Bid() { Item = item, User = user, Value = bid });
                item.Sold = true;
                item.BuyersName = user.Name;
                user.Wallet -= bid;
                _db.SaveChanges();
            }

            for (int i = 0; i < item.Bids.Count(); i++)
            {
                var userId = item.Bids[i].UserId;
                bidDtos.Add(new BidDto(item.Bids[i].Value, _db.Users.FirstOrDefault(p => p.Id == userId).Name));
            }
            ItemDetailsDto itemDetailsDto = new ItemDetailsDto(
                item.Name,
                item.Description,
                item.PhotoURL,
                bidDtos,
                item.PurchasePrice,
                item.User.Name,
                item.BuyersName
                );

            return itemDetailsDto;          
        }                
    }
}
