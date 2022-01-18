using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArgedeSP.WebUI.Areas.Admin.Models.Hesap.Req
{
    public class SifreGuncelle_Req
    {
        [Required(ErrorMessage ="Eski şifre zorunlu")]
        [MaxLength(20,ErrorMessage ="Şifreniz çok uzun, en fazla 20 karakter")]
        public string EskiSifre { get; set; }
        [Required(ErrorMessage = "Yeni şifre zorunlu")]
        [MaxLength(20, ErrorMessage = "Yeni şifreniz çok uzun, en fazla 20 karakter")]
        public string YeniSifre { get; set; }
        [Required(ErrorMessage = "Şifre tekrarı zorunlu")]
        [Compare("YeniSifre",ErrorMessage ="Şifreler eşleşmiyor")]
        public string YeniSifreTekrar { get; set; }
    }
}
