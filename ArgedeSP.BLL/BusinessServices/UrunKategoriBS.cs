
using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Helpers.Extantions;
using ArgedeSP.Contracts.Interfaces.BusinessLogicLayers;
using ArgedeSP.Contracts.Interfaces.Repositories;
using ArgedeSP.Contracts.Models.Common;
using ArgedeSP.Contracts.Models.DTO.UrunlerKategori.Req;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.BLL.BusinessServices
{
    public class UrunKategoriBS : IUrunKategoriBS
    {
        private IUrunKategoriRepository _urunKategoriRepository;

        public UrunKategoriBS(IUrunKategoriRepository urunKategoriRepository)
        {
            if (urunKategoriRepository != null)
                this._urunKategoriRepository = urunKategoriRepository;
        }

        public async Task<OperationResult> UrunKategoriGuncelle(UrunKategoriEkle_REQ inputEt)
        {
            UrunKategori urunKategori_KONTROL = await _urunKategoriRepository.GetByIdAsync(inputEt.Id);
            if (urunKategori_KONTROL == null)
            {
                return OperationResult.Error(MesajKodu.ProjeKategoriBulunamadi);
            }

            UrunKategori urunKategori_SEOKONTROL = await _urunKategoriRepository.FindAsync(x => x.SeoUrl == inputEt.SeoUrl.FriendlyUrl());// Asağıdaki sorguyu burda halledebilirdik ancak performanslı olması için kontrolu bu tarafta yaptık
            if (urunKategori_SEOKONTROL != null && urunKategori_KONTROL.Id != urunKategori_SEOKONTROL.Id)
                return OperationResult.Error(MesajKodu.SeoUrlZatenVar);


            urunKategori_KONTROL.KisaAciklama = inputEt.KisaAciklama;
            urunKategori_KONTROL.Ad = inputEt.Ad;
            urunKategori_KONTROL.ArkaPlanResim = inputEt.ArkaPlanResim;
            urunKategori_KONTROL.KisaAciklama = inputEt.KisaAciklama;
            urunKategori_KONTROL.UstId = inputEt.UstId;
            urunKategori_KONTROL.Resim = inputEt.Resim;
            urunKategori_KONTROL.NavbarResim = inputEt.NavbarResim;
            urunKategori_KONTROL.SeoUrl = inputEt.SeoUrl;
            urunKategori_KONTROL.UzunAciklama = inputEt.UzunAciklama;
            urunKategori_KONTROL.Dil = inputEt.Dil;
            urunKategori_KONTROL.AnaDilcesi = inputEt.AnaDilcesi;

            await _urunKategoriRepository.UpdateAsync(urunKategori_KONTROL, true);

            return OperationResult.Success(MesajKodu.UrunKategoriGuncellendi);
        }

        public async Task<OperationResult> UrunKategorileriGetir()
        {
            return OperationResult.Success(await _urunKategoriRepository.GetAllAsync());

        }

        public OperationResult UrunKategorileriGetir(Dil dil, int adet)
        {
            try
            {
                IList<UrunKategori> urunKategorileri = _urunKategoriRepository.GetAllIncluding().Include(x => x.Urunler).Include(x=>x.UstKategori).Where(x => x.Dil == dil).ToList();
                return OperationResult.Success(urunKategorileri);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{nameof(UrunKategorileriGetir)} fonksiyonunda hata");
                return OperationResult.Error(Enums.MesajKodu.BeklenmedikHata);
            }
        }

        public OperationResult UrunKategorileriniSayfala(int sayfa, int sayfaBoyutu, int id, string ad, string seoUrl)
        {
            return OperationResult.Success(_urunKategoriRepository.SayfalaAramaIle(sayfa, sayfaBoyutu, id, ad, seoUrl));
        }

        public async Task<OperationResult> UrunKategorisiEkle(UrunKategoriEkle_REQ inputEt)
        {
            UrunKategori urunKategoriKontrol = _urunKategoriRepository.Find(x => x.SeoUrl == inputEt.SeoUrl);
            if (urunKategoriKontrol != null)
            {
                return OperationResult.Error(MesajKodu.SeoUrlZatenVar);
            }

            return OperationResult.Success(await _urunKategoriRepository.InsertAsync(new UrunKategori
            {
                Ad = inputEt.Ad,
                ArkaPlanResim = inputEt.ArkaPlanResim,
                KisaAciklama = inputEt.KisaAciklama,
                UstId = inputEt.UstId,
                 Resim = inputEt.Resim,
                NavbarResim = inputEt.NavbarResim,
                SeoUrl = inputEt.SeoUrl.FriendlyUrl(),
                UzunAciklama = inputEt.UzunAciklama,
                Dil = inputEt.Dil,
                AnaDilcesi = inputEt.AnaDilcesi
            }, true));
        }

        public async Task<OperationResult> UrunKategorisiGetirIdIle(int id)
        {
            return OperationResult.Success(await _urunKategoriRepository.GetByIdAsync(id));
        }

        public async Task<OperationResult> UrunKategoriSil(int id)
        {

            UrunKategori urunKategoriKontrol = _urunKategoriRepository.GetById(id);
            if (urunKategoriKontrol == null)
            {
                return OperationResult.Error(MesajKodu.UrunKategoriBulunamadı);
            }

            await _urunKategoriRepository.DeleteAsync(id, true);
            return OperationResult.Success();
        }

    }
}
