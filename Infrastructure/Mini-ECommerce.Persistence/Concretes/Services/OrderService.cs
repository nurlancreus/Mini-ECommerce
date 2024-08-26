using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.DTOs.Order;
using Mini_ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Persistence.Concretes.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderWriteRepository _orderWriteRepository;

        public OrderService(IOrderWriteRepository orderWriteRepository)
        {
            _orderWriteRepository = orderWriteRepository;
        }

        public async Task CreateOrderAsync(CreateOrderDTO createOrder)
        {

            if (!Guid.TryParse(createOrder.BasketId, out var parsedBasketId))
                throw new ArgumentException("Invalid product ID format.", nameof(createOrder));

            var orderCode = (new Random().NextDouble() * 10000).ToString();
            orderCode = orderCode.Substring(orderCode.IndexOf(".") + 1, orderCode.Length - orderCode.IndexOf(".") - 1);

            var address = new Address()
            {
                Country = createOrder.Address.Country,
                Street = createOrder.Address.Street,
                City = createOrder.Address.City,
                PostalCode = createOrder.Address.PostalCode,
                State = createOrder.Address.State,
            };

            await _orderWriteRepository.AddAsync(new()
            {
                Id = parsedBasketId, // we did add basketId as Id column in dbContext modelbuilder
                Description = createOrder.Description,
                OrderCode = orderCode,
                Address = address,
            });
        }

        public Task<GetOrderDTO> GetOrderByIdAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
