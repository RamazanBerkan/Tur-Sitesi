using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArgedeSP.WebUI.Areas.Admin.Models.Hesap.Req
{
    public class Kaydol_Req
    {
        [Required(ErrorMessage = "Ad zorunlu")]
        [MaxLength(30, ErrorMessage = "Ad çok uzun")]
        public string Ad { get; set; }

        [Required(ErrorMessage = " Soyad zorunlu")]
        [MaxLength(30, ErrorMessage = "Soyad çok uzun")]
        public string Soyad { get; set; }

        [Required(ErrorMessage = "E-mail adresi zorunlu")]
        [MaxLength(60, ErrorMessage = "E-mail adresi çok uzun")]
        [EmailAddress(ErrorMessage = "E-mail adresi geçersiz")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Şifre zorunlu")]
        [MinLength(6, ErrorMessage = "Şifre çok kısa")]
        [MaxLength(15, ErrorMessage = "Şifre çok uzun")]
        public string Sifre { get; set; }

        [Required(ErrorMessage = "Şifre tekrarı zorunlu")]
        [Compare("Sifre", ErrorMessage = "Şifreler uyuşmuyor")]
        public string SifreTekrar { get; set; }
        public string BeklenmedikHatalar { get; set; }

    }
}
