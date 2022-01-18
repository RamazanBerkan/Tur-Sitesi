using ArgedeSP.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArgedeSP.WebUI.Models
{
    public class BlogViewModel
    {
        public List<SliderResim> SliderResim { get; set; }
        public List<Blog> Bloglar { get; set; }
        public List<BlogKategori> BlogKategorileri { get; set; }
    }
}
