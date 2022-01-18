
using ArgedeSP.Contracts.DTO.BlogKategori.Req;
using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Helpers.Extantions;
using ArgedeSP.Contracts.Interfaces.BusinessLogicLayers;
using ArgedeSP.Contracts.Interfaces.Repositories;
using ArgedeSP.Contracts.Models.Common;
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
    public class BlogKategoriBS : IBlogKategoriBS
    {
        private IBlogKategoriRepository _blogKategoriRepository;

        public BlogKategoriBS(IBlogKategoriRepository blogKategoriRepository)
        {
            if (blogKategoriRepository != null)
                this._blogKategoriRepository = blogKategoriRepository;
        }

        public async Task<OperationResult> BlogKategoriGuncelle(BlogKategoriEkle_REQ inputEt)
        {
            BlogKategori blogKategori_KONTROL = await _blogKategoriRepository.GetByIdAsync(inputEt.Id);
            if (blogKategori_KONTROL == null)
            {
                return OperationResult.Error(MesajKodu.BlogKategoriBulunamadı);
            }

            //BlogKategori blogKategori_SEOKONTROL = await _blogKategoriRepository.FindAsync(x => x.SeoUrl == inputEt.SeoUrl.FriendlyUrl());// Asağıdaki sorguyu burda halledebilirdik ancak performanslı olması için kontrolu bu tarafta yaptık
            //if (blogKategori_SEOKONTROL != null && blogKategori_KONTROL.Id != blogKategori_SEOKONTROL.Id)
            //    return OperationResult.Error(MesajKodu.SeoUrlZatenVar);


            blogKategori_KONTROL.KisaAciklama = inputEt.KisaAciklama;
            blogKategori_KONTROL.BlogKategoriAdi = inputEt.BlogKategoriAdi;
            blogKategori_KONTROL.ArkaPlanResim = inputEt.ArkaPlanResim;
            blogKategori_KONTROL.KisaAciklama = inputEt.KisaAciklama;
            blogKategori_KONTROL.Resim = inputEt.Resim;
            blogKategori_KONTROL.SeoUrl = inputEt.SeoUrl;
            blogKategori_KONTROL.UzunAciklama = inputEt.UzunAciklama;
            blogKategori_KONTROL.Dil = inputEt.Dil;
            blogKategori_KONTROL.AnaDilcesi = inputEt.AnaDilcesi;

            await _blogKategoriRepository.UpdateAsync(blogKategori_KONTROL, true);

            return OperationResult.Success(MesajKodu.BlogKategoriGuncellendi);
        }
        public async Task<OperationResult> BlogKategorileriGetir()
        {
            return OperationResult.Success(await _blogKategoriRepository.GetAllAsync());

        }
        public OperationResult BlogKategorileriGetir(Dil dil, int adet)
        {
            try
            {
                IList<BlogKategori> blogKategorileri = _blogKategoriRepository.GetAllIncluding().Include(x => x.Bloglar).Where(x => x.Dil == dil).ToList();
                return OperationResult.Success(blogKategorileri);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{nameof(BlogKategorileriGetir)} fonksiyonunda hata");
                return OperationResult.Error(Enums.MesajKodu.BeklenmedikHata);
            }
        }
        public OperationResult BlogKategorileriniSayfala(int sayfa, int sayfaBoyutu, int id, string ad, string seoUrl, Dil dil)
        {
            return OperationResult.Success(_blogKategoriRepository.SayfalaAramaIle(sayfa, sayfaBoyutu, id, ad, seoUrl, dil));
        }
        public async Task<OperationResult> BlogKategorisiEkle(BlogKategoriEkle_REQ inputEt)
        {
            //BlogKategori blogKategoriKontrol = _blogKategoriRepository.Find(x => x.SeoUrl == inputEt.SeoUrl);
            //if (blogKategoriKontrol != null)
            //{
            //    return OperationResult.Error(MesajKodu.SeoUrlZatenVar);
            //}
            var kategori = await _blogKategoriRepository.InsertAsync(new BlogKategori
            {
                BlogKategoriAdi = inputEt.BlogKategoriAdi,
                ArkaPlanResim = inputEt.ArkaPlanResim,
                KisaAciklama = inputEt.KisaAciklama,
                Resim = inputEt.Resim,
                SeoUrl = inputEt.SeoUrl.FriendlyUrl(),
                UzunAciklama = inputEt.UzunAciklama,
                AnaDilcesi = inputEt.AnaDilcesi,
                Dil = inputEt.Dil,
                OlusturmaTarihi = DateTime.Now
            }, true);
            return OperationResult.Success(((EntityEntry)kategori).Entity);
        }
        public async Task<OperationResult> BlogKategorisiGetirIdIle(int id)
        {
            return OperationResult.Success(await _blogKategoriRepository.GetByIdAsync(id));
        }
        public async Task<OperationResult> BlogKategoriSil(int id)
        {

            BlogKategori blogKategoriKontrol = _blogKategoriRepository.GetById(id);
            if (blogKategoriKontrol == null)
            {
                return OperationResult.Error(MesajKodu.BlogKategoriBulunamadı);
            }

            await _blogKategoriRepository.DeleteAsync(id, true);
            return OperationResult.Success();
        }

    }
}
