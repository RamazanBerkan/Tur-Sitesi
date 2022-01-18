using ArgedeSP.Contracts.Configurations;
using ArgedeSP.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.Contracts.Models.DTO.Urunler.Req
{
    public class UrunEkle_REQ
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bu alan zorunludur")]
        public string UrunAdi { get; set; }
        [Required(ErrorMessage = "Bu alan zorunludur")]
        //public string UrunModel { get; set; }
        public string Keywords { get; set; }
        public string SeoUrl { get; set; }
        public string UrunKodu { get; set; }
        public string AnaResim { get; set; } = DefaultResimYollari.ResimYok;
        public int UrunSira { get; set; }
        public string DetayAciklama { get; set; }
        public string KisaAciklama { get; set; }
        public string UzunAciklama { get; set; }
        public string TeknikOzellikler { get; set; }
        public string NasilCalisir { get; set; }
        public bool Durum { get; set; } = true;
        [Required(ErrorMessage = "Bu alan zorunludur")]
        public int? UrunKategoriId { get; set; }
        public string Fiyat { get; set; }
        public string Indirim { get; set; }
        public string IndirimliFiyat { get; set; }
        public List<UrunDokumani> UrunDokumanlari { get; set; }
        public List<string> Dokumanlar { get; set; }
        public List<string> ResimGalerisi { get; set; }

        public Dil Dil { get; set; }

        public string BeklenmedikHata { get; set; }

        public UrunEkle_REQ()
        {
            UrunDokumanlari = new List<UrunDokumani>();
            Dokumanlar = new List<string>();
            ResimGalerisi = new List<string>();
        }
    }
}
