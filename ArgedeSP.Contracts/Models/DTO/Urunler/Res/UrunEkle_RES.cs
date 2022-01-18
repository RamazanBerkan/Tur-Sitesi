using ArgedeSP.Contracts.Models.DTO.Urunler.Req;
using ArgedeSP.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArgedeSP.Contracts.Models.DTO.Urunler.Res
{
    public class UrunEkle_RES
    {
        public UrunEkle_REQ UrunEkle_REQ { get; set; }
        public List<UrunDokumani> urunDokumanlari { get; set; }
        public UrunDokumani Dokuman { get; set; }
        public List<UrunKategori> UrunKategorileri { get; set; }
        public Urun Urun { get; set; }
    }
}
