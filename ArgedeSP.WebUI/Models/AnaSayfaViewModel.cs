using ArgedeSP.Contracts.DTO.Hizmet.Req;
using ArgedeSP.Contracts.DTO.Hizmet.Res;
using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Models.Common;
using ArgedeSP.Contracts.Models.DTO.Iletisim.Req;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArgedeSP.WebUI.Models
{
    public class AnaSayfaViewModel
    {

        public SliderResim BannerResim { get; set; }
        public List<SliderResim> SliderResimleri { get; set; }
        public List<Blog> SonBloglar { get; set; }
        public string ProjeAdi { get; set; }
        public AnaSayfaBanner AnaSayfaBanner { get; set; }
        public List<Hizmet> Hizmetler { get; set; }
        public List<OnHizmet> OnHizmetler { get; set; }
        public List<Urun> Urunler { get; set; }
        public List<UrunKategori> Kategoriler { get; set; }
        public List<BlogKategori> BlogKategorileri { get; set; }
        public List<Haber> Haberler { get;  set; }
        public IletisimForm_REQ IletisimForm_REQ { get; set; }

    }
}
