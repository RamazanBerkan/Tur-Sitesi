using ArgedeSP.Contracts.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.Contracts.Entities
{
    public class Urun : BaseEntity
    {
        public string UrunAdi { get; set; }
        public string Keywords { get; set; }

        //public string UrunModel { get; set; }
        public string UrunKodu { get; set; }
        public string Resim { get; set; }
        public int UrunSira { get; set; }
        public string DetayAciklama { get; set; }
        public string KisaAciklama { get; set; }
        public string UzunAciklama { get; set; }
        public string TeknikOzellikler { get; set; }
        public string NasilCalisir { get; set; }

        public Durum Durum { get; set; }
        public string SeoUrl { get; set; }
        public string Fiyat { get; set; }
        public string IndirimliFiyat { get; set; }
        public string Indirim { get; set; }

        public Dil Dil { get; set; }
        public ICollection<UrunDokumani> UrunDokumanlari { get; set; }
        public ICollection<UrunResimGaleri> UrunResimGalerisi { get; set; }

        public int UrunKategoriId { get; set; }
        [ForeignKey("UrunKategoriId")]
        public UrunKategori UrunKategori { get; set; }
    }
}
