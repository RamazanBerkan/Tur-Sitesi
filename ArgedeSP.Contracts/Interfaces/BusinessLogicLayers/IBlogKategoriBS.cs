
using ArgedeSP.Contracts.DTO.BlogKategori.Req;
using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.Contracts.Interfaces.BusinessLogicLayers
{
    public interface IBlogKategoriBS
    {
        Task<OperationResult> BlogKategorileriGetir();
        OperationResult BlogKategorileriGetir(Enums.Dil dil, int adet);
        Task<OperationResult> BlogKategorisiGetirIdIle(int id);
        Task<OperationResult> BlogKategorisiEkle(BlogKategoriEkle_REQ inputEt);
        Task<OperationResult> BlogKategoriGuncelle(BlogKategoriEkle_REQ inputEt);
        Task<OperationResult> BlogKategoriSil(int id);

        OperationResult BlogKategorileriniSayfala(int sayfa,int sayfaBoyutu, int id, string ad,string seoUrl,Dil dil);

    }
}
