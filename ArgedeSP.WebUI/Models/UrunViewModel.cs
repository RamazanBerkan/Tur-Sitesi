using ArgedeSP.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArgedeSP.WebUI.Models
{
    public class UrunViewModel
    {
      
        public List<Urun> Urunler { get; set; }
        public List<UrunKategori> UrunKategorileri { get; set; }
        public List<SliderResim> SliderResim { get; set; }

        public string altKategoriSeoUrl { get; set; }
        public string anaKategoriSeoUrl { get; set; }
        
        public string filtre { get; set; }
        public string NavbarResim { get; set; }
    }
}
