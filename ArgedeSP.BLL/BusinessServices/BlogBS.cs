using ArgedeSP.Contracts.DTO.Bloglar.Req;
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
    public class BlogBS : IBlogBS
    {
        private IBlogRepository _blogRepository;
        private IBlogKategoriRepository _blogKategoriRepository;

        public BlogBS(IBlogRepository blogRepository,
            IBlogKategoriRepository blogKategoriRepository)
        {
            _blogRepository = blogRepository;
            _blogKategoriRepository = blogKategoriRepository;
        }

        public async Task<OperationResult> BlogGuncelle(BlogEkle_REQ inputEt)
        {
            Blog blog_KONTROL = await _blogRepository.GetByIdAsync(inputEt.Id);
            if (blog_KONTROL == null)
                return OperationResult.Error(MesajKodu.BlogBulunamadı);

            //Blog blog_SEOKONTROL = await _blogRepository.FindAsync(x => x.SeoUrl == inputEt.SeoUrl.FriendlyUrl());// Asağıdaki sorguyu burda halledebilirdik ancak performanslı olması için kontrolu bu tarafta yaptık
            //if (blog_SEOKONTROL != null && blog_SEOKONTROL.Id != blog_SEOKONTROL.Id)
            //    return OperationResult.Error(MesajKodu.SeoUrlZatenVar);

            BlogKategori blogKategoriKONTROL = await _blogKategoriRepository.FindAsync(x => x.Id == inputEt.BlogKategoriId);
            if (blogKategoriKONTROL == null)
                return OperationResult.Error(MesajKodu.BlogKategoriBulunamadı);


            blog_KONTROL.Resim = inputEt.Resim;
            blog_KONTROL.ArkaPlanResim = inputEt.ArkaPlanResim;
            blog_KONTROL.Baslik = inputEt.Baslik;
            blog_KONTROL.KisaAciklama = inputEt.KisaAciklama;
            blog_KONTROL.BlogEtiket = inputEt.BlogEtiket;
            blog_KONTROL.Ses = inputEt.Ses;
            blog_KONTROL.SeoUrl = inputEt.SeoUrl;
            blog_KONTROL.UzunAciklama = inputEt.UzunAciklama;
            blog_KONTROL.BlogKategori = blogKategoriKONTROL;


            await _blogRepository.UpdateAsync(blog_KONTROL, true);

            return OperationResult.Success(MesajKodu.BlogGuncellendi);
        }
        public async Task<OperationResult> BloglariGetir()
        {
            return OperationResult.Success(await _blogRepository.GetAllAsync());

        }
        public OperationResult BloglariSayfala(int sayfa, int sayfaBoyutu, int id, string baslik, Dil dil, string seoUrl, string bkategori)
        {
            return OperationResult.Success(_blogRepository.SayfalaAramaIle(sayfa, sayfaBoyutu, id, baslik, dil, seoUrl, bkategori));
        }
        public async Task<OperationResult> BlogEkle(BlogEkle_REQ inputEt)
        {
            BlogKategori blogKategoriKontrol = _blogKategoriRepository.Find(x => x.Id == inputEt.BlogKategoriId);
            if (blogKategoriKontrol == null)
            {
                return OperationResult.Error(MesajKodu.BlogKategoriBulunamadı);
            }
            var blog = await _blogRepository.InsertAsync(new Blog
            {
                Baslik = inputEt.Baslik,
                ArkaPlanResim = inputEt.ArkaPlanResim,
                KisaAciklama = inputEt.KisaAciklama,
                Resim = inputEt.Resim,
                BlogEtiket = inputEt.BlogEtiket,
                Ses = inputEt.Ses,
                SeoUrl = inputEt.SeoUrl.FriendlyUrl(),
                UzunAciklama = inputEt.UzunAciklama,
                BlogKategori = blogKategoriKontrol,
                Dil = inputEt.Dil,
                OlusturmaTarihi = DateTime.Now

            }, true);

            return OperationResult.Success(((EntityEntry)blog).Entity);
        }
        public async Task<OperationResult> BlogGetirIdIle(int id)
        {
            try
            {
                Blog blog = await _blogRepository.GetAllIncluding().Include(x => x.BlogKategori).FirstOrDefaultAsync(x => x.Id == id);
                return OperationResult.Success(blog);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{nameof(BlogGetirIdIle)} fonksiyonunda hata");
                return OperationResult.Error(Enums.MesajKodu.BeklenmedikHata);
            }
        }
        public async Task<OperationResult> BlogSil(int id)
        {

            Blog blogKontrol = _blogRepository.GetById(id);
            if (blogKontrol == null)
            {
                return OperationResult.Error(MesajKodu.BlogKategoriBulunamadı);
            }

            await _blogRepository.DeleteAsync(id, true);
            return OperationResult.Success();
        }
        public OperationResult BloglariGetir(Dil dil, int adet)
        {
            try
            {
                IList<Blog> bloglar = _blogRepository.GetAllIncluding().OrderByDescending(x => x.OlusturmaTarihi).Where(x => x.Dil == dil).Take(adet).ToList();
                return OperationResult.Success(bloglar);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{nameof(BloglariGetir)} fonksiyonunda hata");
                return OperationResult.Error(Enums.MesajKodu.BeklenmedikHata);
            }
        }
        public OperationResult BloglariGetir(Dil dil, string kategoriSeoUrl, int adet)
        {
            try
            {
                if (kategoriSeoUrl!=null)
                {
                    IList<Blog> bloglar = _blogRepository.GetAllIncluding().OrderBy(x => x.OlusturmaTarihi).Where(x => x.Dil == dil && x.BlogKategori.SeoUrl == kategoriSeoUrl).Take(adet).ToList();
                    return OperationResult.Success(bloglar);
                }
                else
                {
                    IList<Blog> bloglar = _blogRepository.GetAllIncluding().OrderByDescending(x => x.OlusturmaTarihi).Where(x => x.Dil == dil).Take(adet).ToList();
                    return OperationResult.Success(bloglar);
                }


            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{nameof(BloglariGetir)} fonksiyonunda hata");
                return OperationResult.Error(Enums.MesajKodu.BeklenmedikHata);
            }
        }
    }
}