using ArgedeSP.Contracts.Configurations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArgedeSP.Contracts.Models.DTO.Referanslar.Req
{
    public class ReferansEkle_REQ
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Bu alan zorunludur")]
        public string ReferansAdi { get; set; }
        public string Resim { get; set; } = DefaultResimYollari.ResimYok;
        public string ArkaPlanResmi { get; set; } = DefaultResimYollari.ArkaPlanResmiYok;
        public string KisaAciklama { get; set; }
        public string UzunAciklama { get; set; }
        //[Required(ErrorMessage = "Bu alan zorunludur")]
        public string SeoUrl { get; set; }
    }
}
