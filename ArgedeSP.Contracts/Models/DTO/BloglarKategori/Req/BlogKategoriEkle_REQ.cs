using ArgedeSP.Contracts.Configurations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.Contracts.DTO.BlogKategori.Req
{
    public class BlogKategoriEkle_REQ
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bu alan zorunludur")]
        public string BlogKategoriAdi { get; set; }
        public string Resim { get; set; } = DefaultResimYollari.ResimYok;
        public string ArkaPlanResim { get; set; } = DefaultResimYollari.ArkaPlanResmiYok;
        public string KisaAciklama { get; set; }
        public string UzunAciklama { get; set; }
        //[Required(ErrorMessage = "Bu alan zorunludur")]
        public string SeoUrl { get; set; }
        public Dil Dil { get; set; }
        public string AnaDilcesi { get; set; }
    }
}
