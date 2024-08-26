using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.DTOs.Basket;
using Mini_ECommerce.Domain.Entities;
using Mini_ECommerce.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Persistence.Concretes.Services
{
    public class BasketService : IBasketService
    {
        readonly IHttpContextAccessor _httpContextAccessor;
        readonly UserManager<AppUser> _userManager;
        readonly IOrderReadRepository _orderReadRepository;
        readonly IBasketWriteRepository _basketWriteRepository;
        readonly IBasketReadRepository _basketReadRepository;
        readonly IBasketItemWriteRepository _basketItemWriteRepository;
        readonly IBasketItemReadRepository _basketItemReadRepository;
        public BasketService(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, IOrderReadRepository orderReadRepository, IBasketWriteRepository basketWriteRepository, IBasketItemWriteRepository basketItemWriteRepository, IBasketItemReadRepository basketItemReadRepository, IBasketReadRepository basketReadRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _orderReadRepository = orderReadRepository;
            _basketWriteRepository = basketWriteRepository;
            _basketItemWriteRepository = basketItemWriteRepository;
            _basketItemReadRepository = basketItemReadRepository;
            _basketReadRepository = basketReadRepository;
        }

        public Basket? UserActiveBasket => GetTargetBasket().Result;

        public async Task AddItemToBasketAsync(CreateBasketItemDTO basketItem)
        {
            // Validate basketItem input
            if (basketItem == null)
                throw new ArgumentNullException(nameof(basketItem), "Basket item cannot be null.");

            if (string.IsNullOrEmpty(basketItem.ProductId))
                throw new ArgumentException("Product ID cannot be null or empty.", nameof(basketItem));

            if (basketItem.Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(basketItem));

            try
            {
                // Parse product ID and check if valid
                if (!Guid.TryParse(basketItem.ProductId, out var productId))
                    throw new ArgumentException("Invalid product ID format.", nameof(basketItem));

                // Get or create user's basket
                Basket? basket = await GetTargetBasket() ?? throw new InvalidOperationException("User basket could not be found or created.");

                // Check if item already exists in the basket
                var existingBasketItem = await _basketItemReadRepository.GetSingleAsync(bi =>
                    bi.BasketId == basket.Id && bi.ProductId == productId);

                if (existingBasketItem != null)
                {
                    // If the item exists, increment its quantity
                    existingBasketItem.Quantity++;
                }
                else
                {
                    // If the item does not exist, add a new basket item
                    var newBasketItem = new BasketItem
                    {
                        BasketId = basket.Id,
                        ProductId = productId,
                        Quantity = basketItem.Quantity
                    };

                    await _basketItemWriteRepository.AddAsync(newBasketItem);
                }

                // Save changes to the repository
                await _basketItemWriteRepository.SaveAsync();
            }
            catch (Exception ex)
            {
                // Log exception or handle it accordingly
                // Logging mechanism should be added to capture exceptions
                throw new Exception("An error occurred while adding the item to the basket.", ex);
            }
        }


        public async Task<List<BasketItem>> GetBasketItemsAsync()
        {
            // Retrieve the user's target basket
            Basket? basket = await GetTargetBasket() ?? throw new InvalidOperationException("Basket could not be found or created.");

            // Fetch the basket along with its items and associated products
            Basket? result = await _basketReadRepository.Table
                .Include(b => b.BasketItems)
                .ThenInclude(bi => bi.Product)
                .FirstOrDefaultAsync(b => b.Id == basket.Id) ?? throw new InvalidOperationException("The basket was not found in the repository.");

            // If basket items are null, return an empty list to avoid null reference exception
            return result.BasketItems?.ToList() ?? [];
        }


        public async Task RemoveBasketItemAsync(string basketItemId)
        {
            if (string.IsNullOrEmpty(basketItemId))
            {
                throw new ArgumentException("Basket item ID cannot be null or empty.", nameof(basketItemId));
            }

            // Fetch the basket item from the repository
            BasketItem? basketItem = await _basketItemReadRepository.GetByIdAsync(basketItemId) ?? throw new InvalidOperationException($"Basket item with ID {basketItemId} was not found.");

            // Remove the basket item and save changes
            bool isRemoved = _basketItemWriteRepository.Remove(basketItem);

            if (isRemoved)
            {
                await _basketItemWriteRepository.SaveAsync();
            }

            throw new InvalidOperationException("Could not remove basket item.");

        }


        public async Task UpdateQuantityAsync(UpdateBasketItemDTO basketItem)
        {
            if (basketItem == null)
            {
                throw new ArgumentNullException(nameof(basketItem), "Basket item cannot be null.");
            }

            if (string.IsNullOrEmpty(basketItem.BasketItemId))
            {
                throw new ArgumentException("Basket item ID cannot be null or empty.", nameof(basketItem));
            }

            if (basketItem.Quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.", nameof(basketItem));
            }

            // Fetch the basket item from the repository
            BasketItem? existingBasketItem = await _basketItemReadRepository.GetByIdAsync(basketItem.BasketItemId) ?? throw new InvalidOperationException($"Basket item with ID {basketItem.BasketItemId} was not found.");

            // Update the quantity and save changes
            existingBasketItem.Quantity = basketItem.Quantity;

            await _basketItemWriteRepository.SaveAsync();
        }


        private async Task<Basket?> GetTargetBasket()
        {
            // Retrieve the current user's username from the HTTP context
            var username = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;

            // Throw an exception if no username is found
            if (string.IsNullOrEmpty(username))
                throw new InvalidOperationException("No username found in the HTTP context.");

            // Fetch the user from the database, including their baskets
            var user = await _userManager.Users
                .Include(u => u.Baskets)
                .FirstOrDefaultAsync(u => u.UserName == username)
                ?? throw new InvalidOperationException("User not found.");

            // Find the first basket without an order, if any
            var targetBasket = user.Baskets.FirstOrDefault(basket =>
                !_orderReadRepository.Table.Any(order => order.Id == basket.Id));

            // Create a new basket if no basket without an order is found
            if (targetBasket == null)
            {
                targetBasket = new Basket();
                user.Baskets.Add(targetBasket);

                // Save changes to the database
                await _basketWriteRepository.SaveAsync();
            }


            return targetBasket;
        }


    }
}
