using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mini_ECommerce.Application.Attributes;
using Mini_ECommerce.Domain.Enums;
using Mini_ECommerce.Application.Features.Commands.Basket.AddItemToBasket;
using Mini_ECommerce.Application.Features.Commands.Basket.RemoveItemFromBasket;
using Mini_ECommerce.Application.Features.Commands.Basket.UpdateItemQuantity;
using Mini_ECommerce.Application.Features.Queries.Basket.GetBasketItems;

namespace Mini_ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BasketsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AuthorizeDefinition(Menu = AuthorizedMenu.Baskets, ActionType = ActionType.Reading, Definition = "Get All Basket Items")]
        public async Task<IActionResult> GetBasketItems([FromQuery] GetBasketItemsQueryRequest getBasketItemsQuery)
        {
            var response = await _mediator.Send(getBasketItemsQuery);
            return Ok(response);
        }

        [HttpPost]
        [AuthorizeDefinition(Menu = AuthorizedMenu.Baskets, ActionType = ActionType.Writing, Definition = "Add item to the Basket")]
        public async Task<IActionResult> AddItemToBasket(AddItemToBasketCommandRequest addItemToBasketCommandRequest)
        {
            AddItemToBasketCommandResponse response = await _mediator.Send(addItemToBasketCommandRequest);

            return Ok(response);
        }

        [HttpPut]
        [AuthorizeDefinition(Menu = AuthorizedMenu.Baskets, ActionType = ActionType.Updating, Definition = "Update item's quantity in the Basket")]
        public async Task<IActionResult> UpdateQuantity(UpdateItemQuantityCommandRequest updateItemQuantityCommand)
        {
            var response = await _mediator.Send(updateItemQuantityCommand);
            return Ok(response);
        }

        [HttpDelete("{BasketItemId}")]
        [AuthorizeDefinition(Menu = AuthorizedMenu.Baskets, ActionType = ActionType.Deleting, Definition = "Delete item from the Basket")]
        public async Task<IActionResult> RemoveBasketItem([FromRoute] RemoveItemFromBasketCommandRequest removeItemFromBasket)
        {
            var response = await _mediator.Send(removeItemFromBasket);
            return Ok(response);
        }
    }
}
