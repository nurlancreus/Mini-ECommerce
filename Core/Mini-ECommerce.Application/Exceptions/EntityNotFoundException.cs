using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public string EntityName { get; }

        public EntityNotFoundException(string entityName)
            : base($"The entity '{entityName}' was not found.")
        {
            EntityName = entityName;
        }

        public EntityNotFoundException(string entityName, string customMessage)
            : base(customMessage)
        {
            EntityName = entityName;
        }

        public EntityNotFoundException(string entityName, int entityId)
            : base($"The entity '{entityName}' with ID '{entityId}' was not found.")
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
