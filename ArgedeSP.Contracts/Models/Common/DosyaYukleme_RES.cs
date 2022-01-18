using System;
using System.Collections.Generic;
using System.Text;

namespace ArgedeSP.Contracts.Models.Common
{
    public class DosyaYukleme_RES
    {
        public bool BasariliMi { get; set; }
        public string DosyaAdi { get; set; }
        public string DosyaUrl { get; set; }
        public double DosyaBoyutu { get; set; }
    }
}
