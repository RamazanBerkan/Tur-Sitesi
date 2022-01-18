using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Helpers;
using ArgedeSP.Contracts.Interfaces.BusinessLogicLayers;
using ArgedeSP.Contracts.Models.Common;
using ArgedeSP.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.WebUI.Controllers
{
    public class BloglarController : Controller
    {
        private ISliderResimBS _sliderResimBS;
        private IAnahtarDegerBS _anahtarDegerBS;
        private IBlogBS _blogBS;
        private IBlogKategoriBS _blogKategoriBS;
        private Dil SuankiDil;

        public BloglarController(
            ISliderResimBS sliderResimBS,
            IAnahtarDegerBS anahtarDegerBS,
            IBlogBS blogBS,
            IBlogKategoriBS blogKategoriBS)
        {
            _sliderResimBS = sliderResimBS;
            _anahtarDegerBS = anahtarDegerBS;
            _blogKategoriBS = blogKategoriBS;
            _blogBS = blogBS;

            SuankiDil = CultureInfo.CurrentCulture.DilGetir();
        }

        public async Task<IActionResult> Bloglar(string kategoriSeoUrl)
        {
            OperationResult sliderResimleri_OR = await _sliderResimBS.SliderBannerResimleriGetir(SuankiDil, "Bloglar");
            OperationResult bloglar_OR = _blogBS.BloglariGetir(SuankiDil, kategoriSeoUrl, int.MaxValue);
            OperationResult blogKategori_OR = _blogKategoriBS.BlogKategorileriGetir(SuankiDil, int.MaxValue);


            BlogViewModel blogViewModel = new BlogViewModel()
            {
                SliderResim=(List<SliderResim>)sliderResimleri_OR.ReturnObject,
                BlogKategorileri = (List<BlogKategori>)blogKategori_OR.ReturnObject,
                Bloglar = (List<Blog>)bloglar_OR.ReturnObject,
            };


            OperationResult description_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Description);
            OperationResult titlesirketadi_OR = _anahtarDegerBS.AnahtarGetir(Dil.Turkce, Tanimlamalar.ProjeAdi);
            ViewBag.Title = "Eşlik -" + ((AnahtarDeger)titlesirketadi_OR.ReturnObject).Deger;
            ViewBag.Description = ((AnahtarDeger)description_OR.ReturnObject).Deger;
            ViewBag.kategoriSeoUrl = kategoriSeoUrl;

            return View(blogViewModel);
        }
        public async Task<IActionResult> BlogDetayi(int blogId)
        {
            OperationResult blog_OR = await _blogBS.BlogGetirIdIle(blogId);
            OperationResult blogKategori_OR = _blogKategoriBS.BlogKategorileriGetir(SuankiDil, int.MaxValue);
            OperationResult sonSekizBlog_OR = _blogBS.BloglariGetir(SuankiDil, 8);

            BlogDetayiViewModel blogDetayiViewModel = new BlogDetayiViewModel()
            {
                Blog = (Blog)blog_OR.ReturnObject,
                BlogKategorileri = (List<BlogKategori>)blogKategori_OR.ReturnObject,
                 SonBloglar = (List<Blog>)sonSekizBlog_OR.ReturnObject
            };

            ViewBag.KategoriId = blogDetayiViewModel.Blog.BlogKategoriId;

            OperationResult description_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Description);
            OperationResult titlesirketadi_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.TitleSirketAdi);

            switch (SuankiDil)
            {
                default:
                case Dil.Turkce:
                    ViewBag.Title = "Eşlik" + ((AnahtarDeger)titlesirketadi_OR.ReturnObject).Deger;
                    break;

                case Dil.Ingilizce:
                    ViewBag.Title = "Eşlik" + ((AnahtarDeger)titlesirketadi_OR.ReturnObject).Deger;
                    break;

            }

        
            ViewBag.Description = ((AnahtarDeger)description_OR.ReturnObject).Deger;

            return View(blogDetayiViewModel);

        }
    }
}