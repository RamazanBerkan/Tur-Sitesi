using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ArgedeSP.Contracts.Models.Common
{
    public class AnaSayfaBanner
    {
        [Required(ErrorMessage ="Bu alan zorunludur")]
        public string Html { get; set; }
        [Required(ErrorMessage = "Bu alan zorunludur")]
        public string Link { get; set; }
        [Required(ErrorMessage = "Bu alan zorunludur")]
        public string Resim { get; set; }
    }
}
