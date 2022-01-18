using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Helpers.Extantions;
using ArgedeSP.Contracts.Interfaces.BusinessLogicLayers;
using ArgedeSP.Contracts.Interfaces.Repositories;
using ArgedeSP.Contracts.Models.Common;
using ArgedeSP.Contracts.Models.DTO.Referanslar.Req;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.BLL.BusinessServices
{
    public class ReferansBS : IReferansBS
    {
        private IReferansRepository _referansRepository;

        public ReferansBS(IReferansRepository referansRepository)
        {
            _referansRepository = referansRepository;
        }

        public async Task<OperationResult> ReferansGuncelle(ReferansEkle_REQ inputEt)
        {
            Referans referans_KONTROL = await _referansRepository.GetByIdAsync(inputEt.Id);
            if (referans_KONTROL == null)
                return OperationResult.Error(MesajKodu.ProjeBulunamadi);

            //Referans referans_SEOKONTROL = await _referansRepository.FindAsync(x => x.SeoUrl == inputEt.SeoUrl.FriendlyUrl());// Asağıdaki sorguyu burda halledebilirdik ancak performanslı olması için kontrolu bu tarafta yaptık
            //if (referans_SEOKONTROL != null && referans_KONTROL.Id != referans_SEOKONTROL.Id)
            //    return OperationResult.Error(MesajKodu.SeoUrlZatenVar);


            referans_KONTROL.KisaAciklama = inputEt.KisaAciklama;
            referans_KONTROL.ArkaPlanResmi = inputEt.ArkaPlanResmi;
            referans_KONTROL.Resim = inputEt.Resim;
            referans_KONTROL.ReferansAdi = inputEt.ReferansAdi;
            referans_KONTROL.KisaAciklama = inputEt.KisaAciklama;
            referans_KONTROL.SeoUrl = inputEt.SeoUrl;
            referans_KONTROL.UzunAciklama = inputEt.UzunAciklama;


            await _referansRepository.UpdateAsync(referans_KONTROL, true);

            return OperationResult.Success(MesajKodu.ReferansGuncellendi);
        }
        public OperationResult ReferanslariSayfala(int sayfa, int sayfaBoyutu, int id, string ad, string seoUrl)
        {
            return OperationResult.Success(_referansRepository.SayfalaAramaIle(sayfa, sayfaBoyutu, id, ad, seoUrl));
        }
        public async Task<OperationResult> ReferansEkle(ReferansEkle_REQ inputEt)
        {
            //Referans referansKontrol = _referansRepository.Find(x => x.SeoUrl == inputEt.SeoUrl);
            //if (referansKontrol != null)
            //{
            //    return OperationResult.Error(MesajKodu.SeoUrlZatenVar);
            //}

            return OperationResult.Success(await _referansRepository.InsertAsync(new Referans
            {
                ReferansAdi = inputEt.ReferansAdi,
                ArkaPlanResmi = inputEt.ArkaPlanResmi,
                KisaAciklama = inputEt.KisaAciklama,
                Resim = inputEt.Resim,
                SeoUrl = inputEt.SeoUrl.FriendlyUrl(),
                UzunAciklama = inputEt.UzunAciklama,
            }, true));
        }
        public async Task<OperationResult> ReferanslariGetirIdIle(int id)
        {
            return OperationResult.Success(await _referansRepository.GetByIdAsync(id));
        }
        public async Task<OperationResult> ReferansSil(int id)
        {

            Referans referansKontrol = _referansRepository.GetById(id);
            if (referansKontrol == null)
            {
                return OperationResult.Error(MesajKodu.ReferansBulunamadi);
            }

            await _referansRepository.DeleteAsync(id, true);
            return OperationResult.Success();
        }
        public async Task<OperationResult> ReferanslariGetir(Dil SuankiDil)
        {
            return OperationResult.Success(await _referansRepository.GetAllAsync());

        }
     
    }
}