using System;
using System.Collections.Generic;
using System.Text;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.Contracts.Models.Common
{
    public class Genel_RES
    {
        public bool BasariliMi { get; set; }
        public string Mesaj { get; set; }
        public object Model { get; set; }
        public string InputName { get; set; }
        public MesajKodu HataKodu { get; set; }
        public List<string> Mesajlar { get; set; }
    }
}
