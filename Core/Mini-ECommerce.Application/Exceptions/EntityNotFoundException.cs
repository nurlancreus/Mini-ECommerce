﻿using Mini_ECommerce.Application.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Exceptions
{
    public class EntityNotFoundException : BaseException
    {
        public string EntityName { get; }

        public EntityNotFoundException(string entityName)
            : base($"The entity '{entityName}' was not found.")
        {
            EntityName = entityName;
        }

        public EntityNotFoundException(string entityName, string customMessage)
            : base(HttpStatusCode.NotFound,customMessage)
        {
            EntityName = entityName;
        }

        public EntityNotFoundException(string entityName, int entityId)
            : base(HttpStatusCode.NotFound, $"The entity '{entityName}' with ID '{entityId}' was not found.")
        {
            EntityName = entityName;
        }

        public EntityNotFoundException(string entityName, int entityId, string customMessage)
            : base(customMessage)
        {
            EntityName = entityName;
        }
    }
}
