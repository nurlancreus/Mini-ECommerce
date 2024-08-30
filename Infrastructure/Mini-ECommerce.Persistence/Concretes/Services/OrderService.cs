using Microsoft.EntityFrameworkCore;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.DTOs.Address;
using Mini_ECommerce.Application.DTOs.Basket;
using Mini_ECommerce.Application.DTOs.Customer;
using Mini_ECommerce.Application.DTOs.Order;
using Mini_ECommerce.Application.DTOs.Pagination;
using Mini_ECommerce.Application.Exceptions;
using Mini_ECommerce.Domain.Entities;
using Mini_ECommerce.Domain.Entities.Identity;
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
        private readonly ICompletedOrderReadRepository _completedOrderReadRepository;
        private readonly ICompletedOrderWriteRepository _completedOrderWriteRepository;

        public OrderService(IOrderWriteRepository orderWriteRepository, IOrderReadRepository orderReadRepository, IPaginationService paginationService, IBasketReadRepository basketReadRepository, IBasketItemReadRepository basketItemReadRepository)
        {
            _orderWriteRepository = orderWriteRepository;
            _orderReadRepository = orderReadRepository;
            _paginationService = paginationService;
            _basketReadRepository = basketReadRepository;
            _basketItemReadRepository = basketItemReadRepository;
        }

        public async Task<(bool IsSuccess, CompletedOrderDTO? CompletedOrder)> CompleteOrderAsync(string id)
        {
            if (!Guid.TryParse(id, out Guid orderId))
            {
                // Invalid GUID format
                return (false, null);
            }

            // Retrieve the order by ID with related data
            var order = await _orderReadRepository.Table
                .Include(o => o.Basket)
                .ThenInclude(b => b.User)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                // Order not found
                return (false, null);
            }

            // Create a new completed order record
            var completedOrder = new CompletedOrder
            {
                OrderId = orderId
            };

            // Add the completed order to the repository
            await _completedOrderWriteRepository.AddAsync(completedOrder);

            // Save changes to the repository
            bool isSaved = await _completedOrderWriteRepository.SaveAsync() > 0;

            // Prepare the DTO for the response
            var completedOrderDTO = new CompletedOrderDTO
            {
                OrderCode = order.OrderCode,
                OrderDate = order.CreatedAt,
                Username = order.Basket.User.UserName,
                Email = order.Basket.User.Email
            };

            return (isSaved, completedOrderDTO);
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
            // Deconstruct the result
            var (totalItems, pageSize, currentPage, totalPages, paginatedQuery) = await GetPaginatedOrdersAsync(page, size);

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
                    isCompleted = _completedOrderReadRepository.Table.AnyAsync(co => co.Id == o.Id).Result,
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

        public async Task<List<GetCustomerDTO>> GetCustomersAsync(int page, int size)
        {
            var completedOrderIds = await _completedOrderReadRepository.Table
                .Select(co => co.OrderId)
                .ToListAsync();

            var ordersQuery = _orderReadRepository.GetAll()
                .Where(o => completedOrderIds.Contains(o.Id))
                .Include(o => o.Basket)
                .ThenInclude(b => b.User)
                .AsNoTracking();

            var orders = ordersQuery.Include(o => o.Basket).ThenInclude(b => b.User);

            var customersQuery = orders.Select(o => o.Basket.User).Distinct();

            var paginationRequest = new PaginationRequestDTO()
            {
                Page = page,
                PageSize = size,
            };

            var (totalItems, pageSize, currentPage, totalPages, paginatedQuery) = await _paginationService.ConfigurePaginationAsync(paginationRequest, customersQuery);

            List<GetCustomerDTO> customers = await paginatedQuery.Select(c => new GetCustomerDTO()
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email!,
                UserName = c.UserName!
            }).ToListAsync();

            return customers;
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
                isCompleted = await _completedOrderReadRepository.Table.AnyAsync(co => co.Id == order.Id),
                TotalPrice = order.Basket.BasketItems.Sum(bi => bi.Quantity * bi.Product.Price)
            };
        }

        private async Task<PaginationResponseDTO<Order>> GetPaginatedOrdersAsync(int page, int size)
        {
            var query = _orderReadRepository.GetAll(false);

            var paginationRequest = new PaginationRequestDTO()
            {
                Page = page,
                PageSize = size,
            };

            return await _paginationService.ConfigurePaginationAsync(paginationRequest, query);
        }
    }
}
