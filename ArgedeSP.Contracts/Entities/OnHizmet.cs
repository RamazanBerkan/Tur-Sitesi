using ArgedeSP.Contracts.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.Contracts.Entities
{
    public class OnHizmet:BaseEntity
    {
        public string OnHizmetAdi { get; set; }
        public string Resim { get; set; }
        public string ArkaPlanResim { get; set; }
        public string KisaAciklama { get; set; }
        public string UzunAciklama { get; set; }
        public string SeoUrl { get; set; }
        public Dil Dil { get; set; }
        //Test
    }
}
