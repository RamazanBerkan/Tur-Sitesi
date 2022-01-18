using ArgedeSP.Contracts.Configurations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.Contracts.DTO.Hizmet.Req
{
    public class HizmetEkle_REQ
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Bu alan zorunludur")]
        public string HizmetAdi { get; set; }
        public string Resim { get; set; } = DefaultResimYollari.ResimYok;
        public string UzunAciklama { get; set; }
        public string ArkaplanResim { get; set; }
        public int Siralama { get; set; }
         [Required(ErrorMessage = "Bu alan zorunludur")]
        public string SeoUrl { get; set; }
        [Required(ErrorMessage = "Bu alan zorunludur")]
        public Dil Dil { get; set; }
    }
}
