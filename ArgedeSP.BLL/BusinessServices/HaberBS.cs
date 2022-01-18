using ArgedeSP.Contracts.DTO.Bloglar.Req;
using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Helpers.Extantions;
using ArgedeSP.Contracts.Interfaces.BusinessLogicLayers;
using ArgedeSP.Contracts.Interfaces.Repositories;
using ArgedeSP.Contracts.Models.Common;
using ArgedeSP.Contracts.Models.DTO.Haberler.Req;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.BLL.BusinessServices
{
    public class HaberBS : IHaberBS
    {
        private IHaberRepository _haberRepository;

        public HaberBS(
            IHaberRepository haberRepository)
        {
            _haberRepository = haberRepository;
        }

        public async Task<OperationResult> HaberGuncelle(HaberEkle_REQ inputEt)
        {
            Haber haber_KONTROL = await _haberRepository.GetByIdAsync(inputEt.Id);
            if (haber_KONTROL == null)
                return OperationResult.Error(MesajKodu.HaberBulunamadi);

            haber_KONTROL.Resim = inputEt.Resim;
            haber_KONTROL.Baslik = inputEt.Baslik;
            haber_KONTROL.KisaAciklama = inputEt.KisaAciklama;
            haber_KONTROL.UzunAciklama = inputEt.UzunAciklama;
            haber_KONTROL.Dil = inputEt.Dil;

            await _haberRepository.UpdateAsync(haber_KONTROL, true);

            return OperationResult.Success(MesajKodu.BlogGuncellendi);
        }
        public async Task<OperationResult> HaberleriGetir()
        {
            return OperationResult.Success(await _haberRepository.GetAllAsync());

        }
        public OperationResult HaberleriSayfala(int sayfa, int sayfaBoyutu, int id, string baslik, Dil dil)
        {
            return OperationResult.Success(_haberRepository.SayfalaAramaIle(sayfa, sayfaBoyutu, id, baslik, dil));
        }
        public async Task<OperationResult> HaberEkle(HaberEkle_REQ inputEt)
        {
            var haber = await _haberRepository.InsertAsync(new Haber
            {
                Baslik = inputEt.Baslik,
                KisaAciklama = inputEt.KisaAciklama,
                Resim = inputEt.Resim,
                UzunAciklama = inputEt.UzunAciklama,
                Dil = inputEt.Dil,
                OlusturmaTarihi = DateTime.Now
            }, true);

            return OperationResult.Success(((EntityEntry)haber).Entity);
        }
        public async Task<OperationResult> HaberGetirIdIle(int id)
        {
            try
            {
                Haber haber = await _haberRepository.FindAsync(x => x.Id == id);
                return OperationResult.Success(haber);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{nameof(HaberGetirIdIle)} fonksiyonunda hata");
                return OperationResult.Error(Enums.MesajKodu.BeklenmedikHata);
            }
        }
        public async Task<OperationResult> HaberSil(int id)
        {
            await _haberRepository.DeleteAsync(id, true);
            return OperationResult.Success();
        }
        public OperationResult HaberleriGetir(Dil dil, int adet)
        {
            try
            {
                IList<Haber> haberler = _haberRepository.GetAllIncluding().OrderBy(x => x.OlusturmaTarihi).Where(x => x.Dil == dil).Take(adet).ToList();
                return OperationResult.Success(haberler);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{nameof(HaberleriGetir)} fonksiyonunda hata");
                return OperationResult.Error(Enums.MesajKodu.BeklenmedikHata);
            }
        }
    }
}