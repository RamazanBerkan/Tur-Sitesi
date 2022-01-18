using ArgedeSP.Contracts.Models.DTO.Referanslar.Req;
using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.Contracts.Interfaces.BusinessLogicLayers
{
    public interface IReferansBS
    {
        Task<OperationResult> ReferanslariGetirIdIle(int id);
        Task<OperationResult> ReferansEkle(ReferansEkle_REQ inputEt);
        Task<OperationResult> ReferansGuncelle(ReferansEkle_REQ inputEt);
        Task<OperationResult> ReferansSil(int id);
        Task<OperationResult> ReferanslariGetir(Dil SuankiDil);
        OperationResult ReferanslariSayfala(int sayfa,int sayfaBoyutu, int id, string ad,string seoUrl);
    }
}
