
using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Models.Common;
using ArgedeSP.Contracts.Models.DTO.UrunlerKategori.Req;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.Contracts.Interfaces.BusinessLogicLayers
{
    public interface IUrunKategoriBS
    {
        Task<OperationResult> UrunKategorileriGetir();
        OperationResult UrunKategorileriGetir(Dil dil, int adet);
        Task<OperationResult> UrunKategorisiGetirIdIle(int id);
        Task<OperationResult> UrunKategorisiEkle(UrunKategoriEkle_REQ inputEt);
        Task<OperationResult> UrunKategoriGuncelle(UrunKategoriEkle_REQ inputEt);
        Task<OperationResult> UrunKategoriSil(int id);

        OperationResult UrunKategorileriniSayfala(int sayfa, int sayfaBoyutu, int id, string ad, string seoUrl);
    }
}
