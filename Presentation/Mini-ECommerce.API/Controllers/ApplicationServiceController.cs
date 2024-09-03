using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mini_ECommerce.Application.Abstractions.Services.Application;
using Mini_ECommerce.Application.Attributes;
using Mini_ECommerce.Domain.Enums;

namespace Mini_ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "Admin")]
    public class ApplicationServiceController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicationServiceController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet]
        //[AuthorizeDefinition(Menu = AuthorizedMenu.Application, ActionType = ActionType.Reading, Definition = "Get Authorize Definition Endpoints")]
        public IActionResult Get()
        {
            var result = _applicationService.GetAuthorizeDefinitionEndpoints(typeof(Program));

            return Ok(result);
        }
    }
}
