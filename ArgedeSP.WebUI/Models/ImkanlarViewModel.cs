using ArgedeSP.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArgedeSP.WebUI.Models
{
    public class ImkanlarViewModel
    {
        public List<Hizmet> Imkanlar { get; set; }
        public string SayfaAciklamasi { get; set; }
    }
}
