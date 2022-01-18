using ArgedeSP.Contracts.Configurations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.Contracts.Models.DTO.Haberler.Req
{
    public class HaberEkle_REQ
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bu alan zorunludur")]
        public string Baslik { get; set; }
        public string Resim { get; set; } = DefaultResimYollari.ResimYok;
        public string KisaAciklama { get; set; }
        public string UzunAciklama { get; set; }
        [Required(ErrorMessage = "Bu alan zorunludur")]
        public Dil Dil { get; set; }
    }
}
