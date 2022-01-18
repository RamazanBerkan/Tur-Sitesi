using ArgedeSP.Contracts.DTO.OnHizmet.Req;
using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Helpers.Extantions;
using ArgedeSP.Contracts.Interfaces.BusinessLogicLayers;
using ArgedeSP.Contracts.Interfaces.Repositories;
using ArgedeSP.Contracts.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.BLL.BusinessServices
{
    public class OnHizmetBS : IOnHizmetBS
    {
        private IOnHizmetRepository _OnhizmetRepository;

        public OnHizmetBS(IOnHizmetRepository OnhizmetRepository)
        {
            _OnhizmetRepository = OnhizmetRepository;
        }

        public async Task<OperationResult> OnHizmetGuncelle(OnHizmetEkle_REQ inputEt)
        {
            OnHizmet Onhizmet_KONTROL = await _OnhizmetRepository.GetByIdAsync(inputEt.Id);
            if (Onhizmet_KONTROL == null)
                return OperationResult.Error(MesajKodu.HizmetBulunamadi);

            OnHizmet Onhizmet_SEOKONTROL = await _OnhizmetRepository.FindAsync(x => x.SeoUrl == inputEt.SeoUrl.FriendlyUrl());// Asağıdaki sorguyu burda halledebilirdik ancak performanslı olması için kontrolu bu tarafta yaptık
            if (Onhizmet_SEOKONTROL != null && Onhizmet_KONTROL.Id != Onhizmet_KONTROL.Id)
                return OperationResult.Error(MesajKodu.SeoUrlZatenVar);



            Onhizmet_KONTROL.OnHizmetAdi = inputEt.OnHizmetAdi;
            Onhizmet_KONTROL.Resim = inputEt.Resim;
            Onhizmet_KONTROL.SeoUrl = inputEt.SeoUrl;
            Onhizmet_KONTROL.UzunAciklama = inputEt.UzunAciklama;
            Onhizmet_KONTROL.Dil = inputEt.Dil;

            await _OnhizmetRepository.UpdateAsync(Onhizmet_KONTROL, true);

            return OperationResult.Success(MesajKodu.HizmetGuncellendi);
        }

        public async Task<OperationResult> OnHizmetleriGetir()
        {
            return OperationResult.Success(await _OnhizmetRepository.GetAllAsync());

        }

        public OperationResult OnHizmetleriSayfala(int sayfa, int sayfaBoyutu, int id, string ad, string seoUrl, Dil dil)
        {
            return OperationResult.Success(_OnhizmetRepository.SayfalaAramaIle(sayfa, sayfaBoyutu, id, ad, seoUrl, dil));
        }

        public async Task<OperationResult> OnHizmetEkle(OnHizmetEkle_REQ inputEt)
        {
            OnHizmet onhizmetKontrol = _OnhizmetRepository.Find(x => x.SeoUrl == inputEt.SeoUrl);
            if (onhizmetKontrol != null)
            {
                return OperationResult.Error(MesajKodu.SeoUrlZatenVar);
            }

            return OperationResult.Success(await _OnhizmetRepository.InsertAsync(new OnHizmet
            {
                OnHizmetAdi = inputEt.OnHizmetAdi,
                Resim = inputEt.Resim,
                SeoUrl = inputEt.SeoUrl.FriendlyUrl(),
                UzunAciklama = inputEt.UzunAciklama,
                Dil = inputEt.Dil
            }, true));
        }

        public async Task<OperationResult> OnHizmetGetirIdIle(int id)
        {
            return OperationResult.Success(await _OnhizmetRepository.GetByIdAsync(id));
        }

        public async Task<OperationResult> OnHizmetSil(int id)
        {

            OnHizmet onhizmetKontrol = _OnhizmetRepository.GetById(id);
            if (onhizmetKontrol == null)
            {
                return OperationResult.Error(MesajKodu.HizmetKategoriBulunamadi);
            }

            await _OnhizmetRepository.DeleteAsync(id, true);
            return OperationResult.Success();
        }

        public OperationResult OnHizmetleriGetir(Dil dil)
        {
            return OperationResult.Success(_OnhizmetRepository.GetAllMatched(x => x.Dil == dil));
        }

        public OperationResult OnHizmetGetirSeoUrlIle(string seoUrl)
        {
            if (string.IsNullOrWhiteSpace(seoUrl))
            {
                return OperationResult.Error(MesajKodu.HizmetBulunamadi);
            }

            OnHizmet onhizmet = _OnhizmetRepository.Find(x => x.SeoUrl == seoUrl);
            if (onhizmet == null)
            {
                return OperationResult.Error(MesajKodu.HizmetBulunamadi);
            }

            return OperationResult.Success(onhizmet);
        }
    }
}