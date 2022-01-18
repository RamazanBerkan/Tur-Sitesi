using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArgedeSP.WebUI.Areas.Admin.Models.Hesap.Req
{
    public class Giris_Req
    {
        [Required(ErrorMessage = "Kullanıcı adı veya E-mail girmek zorundasınız")]
        public string KullaniciAdi { get; set; }

        [Required(ErrorMessage = "Şifre alanı zorunlu")]
        [MinLength(6, ErrorMessage = "Şifreniz çok kısa"), MaxLength(12, ErrorMessage = "Şifreniz çok uzun")]
        public string Sifre { get; set; }

    }
}
