using ArgedeSP.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ArgedeSP.Contracts.Models.Common
{
    public class ResimGalerisi
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Bu alan zorunlu")]
        public string ResimYolu { get; set; }
        [Required(ErrorMessage = "Bu alan zorunlu")]
        public int Sira { get; set; }
        [Required(ErrorMessage = "Bu alan zorunlu")]
        public int UrunKategoriId { get; set; }
        public string KategoriAdi { get; set; }

    }
}
