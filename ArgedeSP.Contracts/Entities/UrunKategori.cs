using ArgedeSP.Contracts.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.Contracts.Entities
{
    public class UrunKategori : BaseEntity
    {
        public string Ad { get; set; }
        public string Resim { get; set; }
        public string NavbarResim { get; set; }
        public string ArkaPlanResim { get; set; }
        public string KisaAciklama { get; set; }
        public string UzunAciklama { get; set; }
        public string SeoUrl { get; set; }

        public Dil Dil { get; set; }
        public string AnaDilcesi { get; set; }

        public ICollection<Urun> Urunler { get; set; }

        public int? UstId { get; set; }
        [ForeignKey("UstId")]
        public UrunKategori UstKategori { get; set; }


    }
}
