using Mini_ECommerce.Application.DTOs.Basket;
using Mini_ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Abstractions.Services
{
    public interface IBasketService
    {
        public Task<List<BasketItem>> GetBasketItemsAsync();
        public Task AddItemToBasketAsync(CreateBasketItemDTO basketItem);
        public Task UpdateQuantityAsync(UpdateBasketItemDTO basketItem);
        public Task RemoveBasketItemAsync(string basketItemId);
        public Basket? UserActiveBasket { get; }
    }
}
