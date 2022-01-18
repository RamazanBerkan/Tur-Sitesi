using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArgedeSP.Contracts.Entities
{
    public class KullaniciRol : IdentityUserRole<string>
    {
        public virtual Kullanici User { get; set; }
        public virtual Rol Role { get; set; }
    }
}
