using ArgedeSP.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArgedeSP.WebUI.Models
{
    public class UrunDetayViewModel
    {
        public Urun Urun { get; set; }
        public List<Urun> SizinIcinSectiklerimiz { get; set; }
        public List<SliderResim> SliderResim { get; set; }
        public List<UrunKategori> UrunKategorileri { get; set; }

    }
}
