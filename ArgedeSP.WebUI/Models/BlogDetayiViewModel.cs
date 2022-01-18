using ArgedeSP.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArgedeSP.WebUI.Models
{
    public class BlogDetayiViewModel
    {
        public Blog Blog { get; set; }
        public List<BlogKategori> BlogKategorileri { get; set; }
        public List<Blog> SonBloglar { get; set; }
    }
}
