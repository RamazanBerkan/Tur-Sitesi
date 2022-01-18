using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.Contracts.Models.DTO.Kulanici.Req
{

    public class Kullanici_REQ
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string Resim { get; set; } = "/img/resim-yok.png";
        [Required(ErrorMessage = "Bu alan zorunludur")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Bu alan zorunludur")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur")]
        public string Ad { get; set; }
        [Required(ErrorMessage = "Bu alan zorunludur")]
        public string Soyad { get; set; }
        [Required(ErrorMessage = "Bu alan zorunludur")]
        public string TCKN { get; set; }
        public string Universite { get; set; }
        public string Fakulte { get; set; }
        public string Sinif { get; set; }

        public string OdaNumarasi { get; set; }
        [Required(ErrorMessage = "Bu alan zorunludur")]
        public bool ProfilGizliMi { get; set; } = true;
        [Required(ErrorMessage = "Bu alan zorunludur")]
        public bool Durum { get; set; }
    }
}
