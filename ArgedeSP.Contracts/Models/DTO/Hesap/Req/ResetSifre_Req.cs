using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArgedeSP.WebUI.Areas.Admin.Models.Hesap.Req
{
    public class ResetSifre_Req
    {
        public string DogrulamaKodu { get; set; }

        [Required(ErrorMessage ="E-mail adresi zorunlu")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage ="Şifre zorunlu")]
        [DataType(DataType.Password)]
        [MinLength(6,ErrorMessage ="Şifreniz çok kısa, min 6 karakter")]
        public string Sifre { get; set; }

        [Required(ErrorMessage ="Şifre tekrarı zorunlu")]
        [DataType(DataType.Password)]
        [Compare("Sifre", ErrorMessage = "Şifreler uyuşmadı")]
        [MinLength(6, ErrorMessage = "Şifre tekrarı çok kısa, min 6 karakter")]
        public string SifreTekrar { get; set; }

        public string BeklenmedikHata { get; set; }
    }
}
