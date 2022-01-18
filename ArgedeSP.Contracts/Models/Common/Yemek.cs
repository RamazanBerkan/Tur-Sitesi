using System;
using System.Collections.Generic;
using System.Text;

namespace ArgedeSP.Contracts.Models.Common
{
    public class Yemek
    {
        public string Id { get; set; }
        public string YemekAdi { get; set; }
        public string Kalori { get; set; }
        public List<string> MalzemeListesi { get; set; }
        public string ResimYolu { get; set; }
    }
}
