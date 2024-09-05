using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.File.UploadFiles
{
    public class UploadFileCommandRequest : IRequest<UploadFileCommandResponse>
    {
        public FormFileCollection FormFiles {  get; set; } = [];
        public string PathName { get; set; }
    }
}
