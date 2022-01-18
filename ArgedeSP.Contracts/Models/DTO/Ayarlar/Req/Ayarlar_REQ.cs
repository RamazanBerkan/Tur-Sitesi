using ArgedeSP.Contracts.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ArgedeSP.Contracts.Models.DTO.Ayarlar.Req
{
    public class Ayarlar_REQ
    {
        [Required(ErrorMessage ="Proje adı zorunludur")]
        public string ProjeAdi_TR { get; set; }
        [Required(ErrorMessage = "Proje adı zorunludur")]

        public string SiteAdi { get; set; }
        [Required(ErrorMessage = "Telefon zorunludur")]
        public string Telefon { get; set; }
        [Required(ErrorMessage = "Telefon zorunludur")]
        public string Faks { get; set; }
        [Required(ErrorMessage = "Faks zorunludur")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Adres zorunludur")]
        public string Adres_TR { get; set; }

        public string FooterYazi_TR { get; set; }
        public string FooterYazi_EN { get; set; }

        public AnaSayfaBanner AnaSayfaBanner_TR { get; set; }
        public AnaSayfaBanner AnaSayfaBanner_EN { get; set; }

        [Required(ErrorMessage = "Google map zorunludur")]
        public string Description { get; set; }
        public string MainKeywords { get; set; }
        public string GoogleMap { get; set; }
        public string Instagram { get; set; }
        public string Twitter { get; set; }
        public string Facebook { get; set; }

        public string IletisimAciklama { get; set; }
        public string IletisimAciklamaBaslik { get; set; }
    }
}
