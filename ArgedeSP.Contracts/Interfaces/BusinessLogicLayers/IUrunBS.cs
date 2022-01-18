using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Models.Common;
using ArgedeSP.Contracts.Models.DTO.Urunler.Req;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.Contracts.Interfaces.BusinessLogicLayers
{
    public interface IUrunBS
    {

        Task<OperationResult> UrunGetir(string seoUrl);
        Task<OperationResult> UrunGetir(int urunId);
        Task<OperationResult> UrunGetirSorgusuz(int urunId);
        
        OperationResult AktifUrunleriGetir(Dil SuankiDil, string altKategoriAdi, string filtre);

        OperationResult AktifUrunleriGetirAnsayfa(Dil SuankiDil);
        OperationResult RandomUrunGetir(int adet);
       

        OperationResult RandomKategoriGetir(int adet, Dil SuankiDil, string altKategoriAdi);
        Task<OperationResult> UrunEkle(UrunEkle_REQ inputEt);
        Task<OperationResult> UrunGuncelle(UrunEkle_REQ inputEt);
        Task<OperationResult> UrunSil(int id);

        OperationResult UrunleriSayfala(int sayfa, int sayfaBoyutu, int id, string urunAdi, string urunKategori, Durum durum);
    }
}
