using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ArgedeSP.Contracts.Models.DTO.Hizmet.Req
{
    public class Hizmet_REQ
    {
        [Required(ErrorMessage = "Lütfen sayfayı yenileyiniz")]
        public int AnahtarId { get; set; }
        [Required(ErrorMessage = "Bu alan zorunlu")]
        public string HizmetIcerik { get; set; }
    }
}
