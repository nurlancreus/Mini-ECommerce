using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mini_ECommerce.Application.Abstractions.Services.Application;

namespace Mini_ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationServiceController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicationServiceController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var datas = _applicationService.GetAuthorizeDefinitionEndpoints(typeof(Program));

            return Ok(datas);
        }
    }
}
