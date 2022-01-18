using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.Contracts.Entities
{
    public class Kullanici : IdentityUser
    {
        public string ProfilResmi { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
        public Durum Durum { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string TCKN { get; set; }
        public DateTime EngellenmeTarihi { get; set; }
        public string EngellenmeNedeni { get; set; }

        public IList<KullaniciRol> KullaniciRolleri { get; set; }

        public string AdSoyad()
        {
            return this.Ad + " " + this.Soyad;
        }
    }
}
