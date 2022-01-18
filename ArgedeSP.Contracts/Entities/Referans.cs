using ArgedeSP.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArgedeSP.Contracts.Entities
{
    public class Referans:BaseEntity
    {
        public string ReferansAdi { get; set; }
        public string KisaAciklama { get; set; }
        public string UzunAciklama { get; set; }
        public string ArkaPlanResmi { get; set; }
        public string Resim { get; set; }
        public string SeoUrl { get; set; }
    }
}
