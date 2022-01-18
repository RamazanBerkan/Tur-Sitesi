using ArgedeSP.Contracts.DTO.Hizmet.Req;
using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.Contracts.Interfaces.BusinessLogicLayers
{
    public interface IHizmetBS
    {
        Task<OperationResult> HizmetleriGetir();
        OperationResult HizmetleriGetir(Dil dil);
        Task<OperationResult> HizmetGetirIdIle(int id);
        OperationResult HizmetGetirSeoUrlIle(string seoUrl);
        Task<OperationResult> HizmetEkle(HizmetEkle_REQ inputEt);
        Task<OperationResult> HizmetGuncelle(HizmetEkle_REQ inputEt);
        Task<OperationResult> HizmetSil(int id);

        OperationResult HizmetleriSayfala(int sayfa, int sayfaBoyutu, int id, string hizmetkategoriadi, string seoUrl,Dil dil);
    }
}