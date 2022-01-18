using ArgedeSP.Contracts.Configurations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.Contracts.DTO.Bloglar.Req
{
    public class BlogEkle_REQ
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Bu alan zorunludur")]
        public string Baslik { get; set; }
        public string Resim { get; set; } = DefaultResimYollari.ResimYok;
        public string ArkaPlanResim { get; set; } = DefaultResimYollari.ArkaPlanResmiYok;
        public string KisaAciklama { get; set; }
        public string UzunAciklama { get; set; }
        public string BlogEtiket { get; set; }
        public string Ses { get; set; }
        //[Required(ErrorMessage = "Bu alan zorunludur")]
        public string SeoUrl { get; set; }
        [Required(ErrorMessage = "Bu alan zorunludur")]
        public int? BlogKategoriId { get; set; }
        public Dil Dil { get; set; }
        public string AnaDilcesi { get; set; }

    }
}
