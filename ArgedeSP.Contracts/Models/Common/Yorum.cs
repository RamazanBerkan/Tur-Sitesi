using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ArgedeSP.Contracts.Models.Common
{
    public class Yorum
    {
        public string Id { get; set; }
        public string YorumIcerik { get; set; }
        public string AdSoyad { get; set; }
        public string Isi { get; set; }      
    }
}
