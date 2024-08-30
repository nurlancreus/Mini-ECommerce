using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Mini_ECommerce.Application.Features.Commands.Order.CompleteOrder;
using Mini_ECommerce.Application.Features.Commands.Order.CreateOrder;
using Mini_ECommerce.Application.Features.Queries.Order.GetAllCustomers;
using Mini_ECommerce.Application.Features.Queries.Order.GetAllOrders;
using Mini_ECommerce.Application.Features.Queries.Order.GetOrderById;

namespace Mini_ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{Id}")]

        public async Task<ActionResult> GetOrderById([FromRoute] GetOrderByIdQueryRequest getOrderByIdQueryRequest)
        {
            var response = await _mediator.Send(getOrderByIdQueryRequest);
            return Ok(response);
        }

        [HttpGet]

        public async Task<ActionResult> GetAllOrders([FromQuery] GetAllOrdersQueryRequest getAllOrdersQueryRequest)
        {
            var response = await _mediator.Send(getAllOrdersQueryRequest);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder(CreateOrderCommandRequest createOrderCommandRequest)
        {
            var response = await _mediator.Send(createOrderCommandRequest);
            return Ok(response);
        }

        [HttpGet("complete-order/{Id}")]
        public async Task<ActionResult> CompleteOrder([FromRoute] CompleteOrderCommandRequest completeOrderCommandRequest)
        {
            var response = await _mediator.Send(completeOrderCommandRequest);

            return Ok(response);
        }

        [HttpGet("/customers")]
        public async Task<IActionResult> GetCustomers([FromQuery] GetAllCustomersQueryRequest getAllCustomersQueryRequest)
        {
            var response = _mediator.Send(getAllCustomersQueryRequest);

            return Ok(response);
        }
    }
}
