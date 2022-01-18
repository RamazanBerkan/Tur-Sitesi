using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArgedeSP.Contracts.Entities
{
    public class Rol : IdentityRole
    {
        public ICollection<KullaniciRol> KullaniciRolleri { get; set; }
    }
}
