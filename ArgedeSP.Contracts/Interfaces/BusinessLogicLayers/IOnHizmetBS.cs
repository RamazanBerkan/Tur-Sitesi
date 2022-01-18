using ArgedeSP.Contracts.DTO.OnHizmet.Req;
using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.Contracts.Interfaces.BusinessLogicLayers
{
    public interface IOnHizmetBS
    {
        Task<OperationResult> OnHizmetleriGetir();
        OperationResult OnHizmetleriGetir(Dil dil);
        Task<OperationResult> OnHizmetGetirIdIle(int id);
        OperationResult OnHizmetGetirSeoUrlIle(string seoUrl);
        Task<OperationResult> OnHizmetEkle(OnHizmetEkle_REQ inputEt);
        Task<OperationResult> OnHizmetGuncelle(OnHizmetEkle_REQ inputEt);
        Task<OperationResult> OnHizmetSil(int id);

        OperationResult OnHizmetleriSayfala(int sayfa, int sayfaBoyutu, int id, string hizmetkategoriadi, string seoUrl,Dil dil);
    }
}