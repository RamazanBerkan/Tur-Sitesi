using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ArgedeSP.Contracts.Models.DTO.ResimGalerisi.Req
{
    public class GaleriResim_REQ
    {
        [Required(ErrorMessage = "Resim url zorunlu")]
        public string ResimUrl { get; set; }
        [Required(ErrorMessage = "Resim sıra zorunlu")]
        public int? ResimSira { get; set; }
    }
}
