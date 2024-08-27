using Microsoft.EntityFrameworkCore;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.DTOs.Address;
using Mini_ECommerce.Application.DTOs.Basket;
using Mini_ECommerce.Application.DTOs.Order;
using Mini_ECommerce.Application.DTOs.Pagination;
using Mini_ECommerce.Application.Exceptions;
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
        private readonly IOrderReadRepository _orderReadRepository;
        private readonly IBasketReadRepository _basketReadRepository;
        private readonly IBasketItemReadRepository _basketItemReadRepository;
        private readonly IPaginationService _paginationService;

        public OrderService(IOrderWriteRepository orderWriteRepository, IOrderReadRepository orderReadRepository, IPaginationService paginationService, IBasketReadRepository basketReadRepository, IBasketItemReadRepository basketItemReadRepository)
        {
            _orderWriteRepository = orderWriteRepository;
            _orderReadRepository = orderReadRepository;
            _paginationService = paginationService;
            _basketReadRepository = basketReadRepository;
            _basketItemReadRepository = basketItemReadRepository;
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

        public async Task<GetAllOrdersDTO> GetAllOrdersAsync(int page, int size)
        {
            //var orders = _orderGetRepository.Table.Include(o => o.Address).Include(o => o.Basket);
            var query = _orderReadRepository.GetAll(false);

            // Create pagination request
            var paginationRequest = new PaginationRequestDTO
            {
                Page = page,  // Ensure 'page' is defined and in scope
                PageSize = size  // Ensure 'size' is defined and in scope
            };

            // Call pagination service
            var paginationResult = await _paginationService.ConfigurePaginationAsync(paginationRequest, query);

            // Deconstruct the result
            var (totalItems, pageSize, currentPage, totalPages, paginatedQuery) = paginationResult;

            var orders = await paginatedQuery
                .Include(o => o.Address)
                .Include(o => o.Basket)
                .ThenInclude(b => b.BasketItems)
                .ThenInclude(bi => bi.Product)
                .OrderByDescending(o => o.CreatedAt).ToListAsync();

            return new GetAllOrdersDTO()
            {
                Orders = orders.Select(o => new GetOrderDTO()
                {
                    Description = o.Description,
                    Id = o.Id.ToString(),
                    CreatedAt = o.CreatedAt,
                    OrderCode = o.OrderCode,
                    TotalPrice = o.Basket.BasketItems.Sum(bi => bi.Quantity * bi.Product.Price),
                    Address = new GetAddressDTO()
                    {
                        Id = o.Address.Id.ToString(),
                        Country = o.Address.Country,
                        City = o.Address.City,
                        PostalCode = o.Address.PostalCode,
                        State = o.Address.State,
                        Street = o.Address.Street
                    },
                    BasketItems = o.Basket.BasketItems.Select(bi => new GetBasketItemDTO()
                    {
                        Name = bi.Product.Name,
                        Price = bi.Product.Price,
                        Quantity = bi.Quantity,
                        TotalPrice = bi.Quantity * bi.Product.Price,
                    }).ToList()

                }).ToList(),
                Page = page,
                TotalCount = totalItems,
                TotalPages = totalPages,
            };

        }

        public async Task<GetOrderDTO> GetOrderByIdAsync(string id)
        {
            var order = await _orderReadRepository.GetByIdAsync(id);

            if (order == null)
                throw new EntityNotFoundException(nameof(order), id);

            // Explicitly load the Address reference
            await _orderReadRepository.Table.Entry(order).Reference(o => o.Address).LoadAsync();

            // Explicitly load the Basket reference
            await _orderReadRepository.Table.Entry(order).Reference(o => o.Basket).LoadAsync();

            // Explicitly load the BasketItems collection
            await _basketReadRepository.Table.Entry(order.Basket).Collection(b => b.BasketItems).LoadAsync();

            // Explicitly load the Product reference for each BasketItem
            foreach (var basketItem in order.Basket.BasketItems)
            {
                await _basketItemReadRepository.Table.Entry(basketItem).Reference(bi => bi.Product).LoadAsync();
            }

            return new GetOrderDTO
            {
                Id = order.Id.ToString(),
                Address = new GetAddressDTO()
                {
                    Id = order.Address.Id.ToString(),
                    Country = order.Address.Country,
                    City = order.Address.City,
                    PostalCode = order.Address.PostalCode,
                    State = order.Address.State,
                    Street = order.Address.Street,
                },
                Description = order.Description,
                CreatedAt = order.CreatedAt,
                OrderCode = order.OrderCode,
                BasketItems = order.Basket.BasketItems.Select(bi => new GetBasketItemDTO()
                {
                    Name = bi.Product.Name,
                    Price = bi.Product.Price,
                    Quantity = bi.Quantity,
                    TotalPrice = bi.Quantity * bi.Product.Price
                }).ToList(),
                Completed = false,
                TotalPrice = order.Basket.BasketItems.Sum(bi => bi.Quantity * bi.Product.Price)
            };
        }
    }
}
