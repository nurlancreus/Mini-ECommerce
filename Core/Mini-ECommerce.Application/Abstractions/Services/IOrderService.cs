using Mini_ECommerce.Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Abstractions.Services
{
    public interface IOrderService
    {
        Task CreateOrderAsync(CreateOrderDTO createOrder);
        Task<GetAllOrdersDTO> GetAllOrdersAsync(int page, int size);
        Task<GetOrderDTO> GetOrderByIdAsync(string id);
        //Task<(bool, CompletedOrderDTO)> CompleteOrderAsync(string id);
    }
}
