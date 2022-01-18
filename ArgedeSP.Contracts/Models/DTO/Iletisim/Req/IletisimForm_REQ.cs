using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ArgedeSP.Contracts.Models.DTO.Iletisim.Req
{
    public class IletisimForm_REQ
    {
        [Required(ErrorMessage = "AdZorunlu")]
        [MaxLength(50, ErrorMessage = "AdSoyadCokUzun")]
        public string Ad { get; set; }

        [Required(ErrorMessage = "SoyadZorunlu")]
        [MaxLength(50, ErrorMessage = "AdSoyadCokUzun")]
        public string Soyad { get; set; }

        [Required(ErrorMessage = "TelefonZorunlu")]
        [MaxLength(100,ErrorMessage = "TelefonIsmiCokUzun")]
        public string Telefon { get; set; }

        [Required(ErrorMessage = "PozisyonZorunlu")]
        [MaxLength(50, ErrorMessage = "PozisyonIsmiCokUzun")]
        public string Pozisyon { get; set; }

        [Required(ErrorMessage = "UlkeSeciniz")]
        [MaxLength(150, ErrorMessage = "UlkeCokUzun")]
        public string Ulke { get; set; }

        [Required(ErrorMessage = "EmailZorunlu")]
        [EmailAddress(ErrorMessage = "EmailGecersiz")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Konu")]
        [MaxLength(1000, ErrorMessage = "MesajCokUzun")]
        public string Konu { get; set; }

        [Required(ErrorMessage = "MesajZorunlu")]
        [MaxLength(1000, ErrorMessage = "MesajCokUzun")]
        public string Mesajiniz { get; set; }


        [Required(ErrorMessage = "Şirket")]
        [MaxLength(1000, ErrorMessage = "MesajCokUzun")]
        public string Sirket { get; set; }

        [Required(ErrorMessage = "Fax")]
        [MaxLength(1000, ErrorMessage = "MesajCokUzun")]
        public string Fax { get; set; }
    }
}