using ArgedeSP.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.Contracts.Entities
{
    public class AnahtarDeger:BaseEntity
    {
        public string Anahtar { get; set; }
        public string Deger { get; set; }
        public string GorunenAd { get; set; }
        public Dil Dil { get; set; }
    }
}
