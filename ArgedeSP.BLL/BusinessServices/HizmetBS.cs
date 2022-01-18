using ArgedeSP.Contracts.DTO.Hizmet.Req;
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
    public class HizmetBS : IHizmetBS
    {
        private IHizmetRepository _hizmetRepository;

        public HizmetBS(IHizmetRepository hizmetRepository)
        {
            _hizmetRepository = hizmetRepository;
        }

        public async Task<OperationResult> HizmetGuncelle(HizmetEkle_REQ inputEt)
        {
            Hizmet hizmet_KONTROL = await _hizmetRepository.GetByIdAsync(inputEt.Id);
            if (hizmet_KONTROL == null)
                return OperationResult.Error(MesajKodu.HizmetBulunamadi);

            Hizmet hizmet_SEOKONTROL = await _hizmetRepository.FindAsync(x => x.SeoUrl == inputEt.SeoUrl.FriendlyUrl());// Asağıdaki sorguyu burda halledebilirdik ancak performanslı olması için kontrolu bu tarafta yaptık
            if (hizmet_SEOKONTROL != null && hizmet_KONTROL.Id != hizmet_SEOKONTROL.Id)
                return OperationResult.Error(MesajKodu.SeoUrlZatenVar);



            hizmet_KONTROL.HizmetAdi = inputEt.HizmetAdi;
            hizmet_KONTROL.Resim = inputEt.Resim;
            hizmet_KONTROL.SeoUrl = inputEt.SeoUrl;
            hizmet_KONTROL.ArkaPlanResim = inputEt.ArkaplanResim;
            hizmet_KONTROL.UzunAciklama = inputEt.UzunAciklama;
            hizmet_KONTROL.Siralama = inputEt.Siralama;
            
            hizmet_KONTROL.Dil = inputEt.Dil;

            await _hizmetRepository.UpdateAsync(hizmet_KONTROL, true);

            return OperationResult.Success(MesajKodu.HizmetGuncellendi);
        }

        public async Task<OperationResult> HizmetleriGetir()
        {
            return OperationResult.Success(await _hizmetRepository.GetAllAsync());

        }

        public OperationResult HizmetleriSayfala(int sayfa, int sayfaBoyutu, int id, string ad, string seoUrl, Dil dil)
        {
            return OperationResult.Success(_hizmetRepository.SayfalaAramaIle(sayfa, sayfaBoyutu, id, ad, seoUrl, dil));
        }

        public async Task<OperationResult> HizmetEkle(HizmetEkle_REQ inputEt)
        {
            Hizmet hizmetKontrol = _hizmetRepository.Find(x => x.SeoUrl == inputEt.SeoUrl);
            if (hizmetKontrol != null)
            {
                return OperationResult.Error(MesajKodu.SeoUrlZatenVar);
            }

            return OperationResult.Success(await _hizmetRepository.InsertAsync(new Hizmet
            {
                HizmetAdi = inputEt.HizmetAdi,
                Resim = inputEt.Resim,
                SeoUrl = inputEt.SeoUrl,
                ArkaPlanResim = inputEt.ArkaplanResim,
                UzunAciklama = inputEt.UzunAciklama,
                Siralama = inputEt.Siralama,
            Dil = inputEt.Dil
            }, true));
        }

        public async Task<OperationResult> HizmetGetirIdIle(int id)
        {
            return OperationResult.Success(await _hizmetRepository.GetByIdAsync(id));
        }

        public async Task<OperationResult> HizmetSil(int id)
        {

            Hizmet hizmetKontrol = _hizmetRepository.GetById(id);
            if (hizmetKontrol == null)
            {
                return OperationResult.Error(MesajKodu.HizmetKategoriBulunamadi);
            }

            await _hizmetRepository.DeleteAsync(id, true);
            return OperationResult.Success();
        }

        public OperationResult HizmetleriGetir(Dil dil)
        {
            return OperationResult.Success(_hizmetRepository.GetAllMatched(x => x.Dil == dil));
        }

        public OperationResult HizmetGetirSeoUrlIle(string seoUrl)
        {
            if (string.IsNullOrWhiteSpace(seoUrl))
            {
                return OperationResult.Error(MesajKodu.HizmetBulunamadi);
            }

            Hizmet hizmet = _hizmetRepository.Find(x => x.SeoUrl == seoUrl);
            if (hizmet == null)
            {
                return OperationResult.Error(MesajKodu.HizmetBulunamadi);
            }

            return OperationResult.Success(hizmet);
        }
    }
}