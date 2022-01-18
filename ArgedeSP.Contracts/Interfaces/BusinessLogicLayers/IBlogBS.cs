
using ArgedeSP.Contracts.DTO.Bloglar.Req;
using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.Contracts.Interfaces.BusinessLogicLayers
{
    public interface IBlogBS
    {
        Task<OperationResult> BloglariGetir();
        OperationResult BloglariGetir(Dil dil, int adet);
        OperationResult BloglariGetir(Dil dil, string kategoriSeoUrl, int adet);
        Task<OperationResult> BlogGetirIdIle(int id);
        Task<OperationResult> BlogEkle(BlogEkle_REQ inputEt);
        Task<OperationResult> BlogGuncelle(BlogEkle_REQ inputEt);
        Task<OperationResult> BlogSil(int id);

        OperationResult BloglariSayfala(int sayfa, int sayfaBoyutu, int id, string baslik,Dil dil, string seoUrl, string bkategori);
    }
}
