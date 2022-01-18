using ArgedeSP.Contracts.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ArgedeSP.Contracts.Entities
{
    public class UrunResimGaleri:BaseEntity
    {
        public string ResimUrl { get; set; }

        public int UrunId { get; set; }
        [ForeignKey("UrunId")]
        public Urun Urun { get; set; }
    }
}
