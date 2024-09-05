using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.File.DeleteFile
{
    public class DeleteFileCommandRequest : IRequest<DeleteFileCommandResponse>
    {
       public string Id { get; set; }
    }
}
