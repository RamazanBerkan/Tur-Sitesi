using ArgedeSP.Contracts.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ArgedeSP.Contracts.Entities
{
   public class UrunDokumani : BaseEntity
    {
        public string DokumanAdi { get; set; }
        public string DokumanUrl { get; set; }
       

        public int UrunId { get; set; }
        [ForeignKey("UrunId")]
        public Urun Urun { get; set; }
    }
}
