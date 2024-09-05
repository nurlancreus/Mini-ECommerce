using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mini_ECommerce.Application.Features.Commands.File.DeleteFile;
using Mini_ECommerce.Application.Features.Commands.File.UploadFiles;
using Mini_ECommerce.Application.Features.Queries.File.GetFileById;
using Mini_ECommerce.Application.Features.Queries.File.GetFiles;

namespace Mini_ECommerce.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FilesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm, FromBody] UploadFileCommandRequest uploadFilesCommandRequest)
        {
            var response = await _mediator.Send(uploadFilesCommandRequest);

            return Ok(response);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] DeleteFileCommandRequest deleteFileCommandRequest)
        {
            var response = await _mediator.Send(deleteFileCommandRequest);

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetFiles([FromQuery] GetFilesQueryRequest getFilesQueryRequest)
        {
            var response = await _mediator.Send(getFilesQueryRequest);

            return Ok(response);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetFile([FromRoute] GetFileByIdQueryRequest getFileByIdQueryRequest)
        {
            var response = await _mediator.Send(getFileByIdQueryRequest);

            return Ok(response);
        }
    }
}
