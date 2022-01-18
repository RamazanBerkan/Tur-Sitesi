using ArgedeSP.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.Contracts.Entities
{
    public class Haber:BaseEntity
    {
        public string Baslik { get; set; }
        public string Resim { get; set; }
        public string KisaAciklama { get; set; }
        public string UzunAciklama { get; set; }
        public Dil Dil { get; set; }
    }
}
