using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Models.DTO.Iletisim.Req;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArgedeSP.WebUI.Models
{
    public class IletisimViewModel
    {
        public string Adres { get; set; }
        public string TelefonNo { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string GoogleMap { get; set; }
        public string Description { get; set; }
        public string MainKeywords { get; set; }
        public string ProjeAdi { get; set; }
        public string SiteAdi { get; set; }

        public string IletisimAciklamaBaslik { get; set; }
        public string IletisimAciklama { get; set; }

        public List<SliderResim> SliderResim { get; set; }
        public IletisimForm_REQ IletisimForm_REQ { get; set; }
    }
}
