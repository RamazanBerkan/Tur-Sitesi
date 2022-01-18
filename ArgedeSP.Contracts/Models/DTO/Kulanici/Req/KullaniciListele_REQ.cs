using ArgedeSP.Contracts.Configurations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.Contracts.Models.DTO.Kulanici.Req
{
    public class KullaniciListele_REQ
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Bu alan zorunludur")]
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string TCKN { get; set; }
        public string Universite { get; set; }
        public string Fakulte { get; set; }
        public string Sinif { get; set; }
        public string OdaNumarasi { get; set; }
        [Required(ErrorMessage = "Bu alan zorunludur")]
        public int? Sayfa { get; set; } = 1;
        public int? SayfaBoyutu { get; set; } = int.MaxValue;
        public Durum? Durum { get; set; }    
    }
}
