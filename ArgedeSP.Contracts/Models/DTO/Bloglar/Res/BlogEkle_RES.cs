using ArgedeSP.Contracts.DTO.Bloglar.Req;
using ArgedeSP.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArgedeSP.Contracts.DTO.Bloglar.Res
{
    public class BlogEkle_RES
    {
        public BlogEkle_REQ BlogEkle_REQ { get; set; }
        public List<Entities.BlogKategori> BlogKategorileri { get; set; }
    }
}
