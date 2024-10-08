﻿using Mini_ECommerce.Domain.Entities.Base;
using Mini_ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Domain.Entities
{
    public class AppFile : BaseEntity
    {
        public string FileName { get; set; }
        public string Path { get; set; }
        public StorageType Storage { get; set; }

        [NotMapped]
        public override DateTime? UpdatedAt => null;

    }
}
