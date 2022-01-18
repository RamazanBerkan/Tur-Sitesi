using ArgedeSP.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArgedeSP.Contracts.Entities
{
   public class ProductSpesifications: BaseEntity
    {
        public int AttrId { get; set; }
        public string Value { get; set; }

    }
}
