using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ArgedeSP.Contracts.Models.DTO.Kurumsal.Req
{
    public class Kurumsal_REQ
    {
        [Required(ErrorMessage = "Lütfen sayfayı yenileyiniz")]
        public int AnahtarId { get; set; }
        [Required(ErrorMessage = "Bu alan zorunlu")]
        public string KurumsalIcerik { get; set; }
    }
}
