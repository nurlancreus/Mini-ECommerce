using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mini_ECommerce.Application.Features.Commands.Basket.AddItemToBasket;
using Mini_ECommerce.Application.Features.Commands.Basket.RemoveItemFromBasket;
using Mini_ECommerce.Application.Features.Commands.Basket.UpdateItemQuantity;
using Mini_ECommerce.Application.Features.Queries.Basket.GetBasketItems;

namespace Mini_ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketItemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BasketItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]

        public async Task<IActionResult> GetBasketItems([FromQuery] GetBasketItemsQueryRequest getBasketItemsQuery)
        {
            var response = await _mediator.Send(getBasketItemsQuery);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddItemToBasket(AddItemToBasketCommandRequest addItemToBasketCommandRequest)
        {
            AddItemToBasketCommandResponse response = await _mediator.Send(addItemToBasketCommandRequest);

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateQuantity(UpdateItemQuantityCommandRequest updateItemQuantityCommand)
        {
            var response = await _mediator.Send(updateItemQuantityCommand);
            return Ok(response);
        }

        [HttpDelete("{BasketItemId}")]

        public async Task<IActionResult> RemoveBasketItem([FromRoute] RemoveItemFromBasketCommandRequest removeItemFromBasket)
        {
            var response = await _mediator.Send(removeItemFromBasket);
            return Ok(response);
        }
    }
}
