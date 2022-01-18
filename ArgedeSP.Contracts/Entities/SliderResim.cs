using ArgedeSP.Contracts.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.Contracts.Entities
{
    public class SliderResim : BaseEntity
    {
        public string ResimUrl { get; set; }
        public int ResimSira { get; set; }
        public string AltBaslik { get; set; }
        public string UstBaslik { get; set; }
        public string Link { get; set; }
        [Required(ErrorMessage = "Dil zorunlu")]
        public Dil Dil { get; set; }
        public SliderYeri SliderYeri { get; set; }
    }
}
