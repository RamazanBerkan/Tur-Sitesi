using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Helpers;
using ArgedeSP.Contracts.Interfaces.BusinessLogicLayers;
using ArgedeSP.Contracts.Models.Common;
using ArgedeSP.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.WebUI.Controllers
{
    public class DanismanlikController : Controller
    {
        private ISliderResimBS _sliderResimBS;
        private IBlogBS _blogBS;
        private IAnahtarDegerBS _anahtarDegerBS;
        private IHizmetBS _hizmetBS;
        private IOnHizmetBS _onhizmetBS;
        private IUrunBS _urunBS;
        private IUrunKategoriBS _urunKategoriBS;
        private IHaberBS _haberBS;

        private Dil SuankiDil;

        public DanismanlikController(ISliderResimBS sliderResimBS,
            IBlogBS blogBS,
            IAnahtarDegerBS anahtarDegerBS,
            IHizmetBS hizmetBS,
            IOnHizmetBS onhizmetBS,
            IUrunBS urunBS,
            IUrunKategoriBS urunKategoriBS,
            IHaberBS haberBS)
        {
            _sliderResimBS = sliderResimBS;
            _blogBS = blogBS;
            _anahtarDegerBS = anahtarDegerBS;
            _hizmetBS = hizmetBS;
            _onhizmetBS = onhizmetBS;
            _urunBS = urunBS;
            _urunKategoriBS = urunKategoriBS;
            _haberBS = haberBS;

            SuankiDil = CultureInfo.CurrentCulture.DilGetir();
        }
        public async Task<IActionResult> Index()
        {

            OperationResult sliderResimleri_OR = await _sliderResimBS.SliderResimleriGetir(SuankiDil,SliderYeri.AnasayfaSlider);
            OperationResult sonSekizBlog_OR = _blogBS.BloglariGetir(SuankiDil, 8);
            OperationResult projeAdi = _anahtarDegerBS.AnahtarGetir(SuankiDil, Tanimlamalar.ProjeAdi);
            OperationResult banner = _anahtarDegerBS.AnahtarGetir(SuankiDil, Tanimlamalar.AnaSayfaBanner);
            OperationResult hizmetler_OR = _hizmetBS.HizmetleriGetir(SuankiDil);
            OperationResult onhizmetler_OR = _onhizmetBS.OnHizmetleriGetir(SuankiDil);
            OperationResult urunler_OR = _urunBS.AktifUrunleriGetirAnsayfa(SuankiDil);
            OperationResult kategoriler_QR = _urunKategoriBS.UrunKategorileriGetir(SuankiDil, 4);
            OperationResult haberler_OR = _haberBS.HaberleriGetir(SuankiDil, int.MaxValue);


            AnaSayfaViewModel anaSayfaViewModel = new AnaSayfaViewModel()
            {
                SliderResimleri = (List<SliderResim>)sliderResimleri_OR.ReturnObject,
                SonBloglar = (List<Blog>)sonSekizBlog_OR.ReturnObject,
                ProjeAdi = ((AnahtarDeger)projeAdi.ReturnObject).Deger,
                Hizmetler = (List<Hizmet>)hizmetler_OR.ReturnObject,
                OnHizmetler = (List<OnHizmet>)onhizmetler_OR.ReturnObject,
                Urunler = (List<Urun>)urunler_OR.ReturnObject,
                Kategoriler = (List<UrunKategori>)kategoriler_QR.ReturnObject,
                AnaSayfaBanner = JsonConvert.DeserializeObject<AnaSayfaBanner>(((AnahtarDeger)banner.ReturnObject).Deger),
                Haberler = (List<Haber>)haberler_OR.ReturnObject


            };
            OperationResult description_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Description);
            OperationResult titlesirketadi_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.TitleSirketAdi);
            OperationResult mainkeywords_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.MainKeywords);


            switch (SuankiDil)
            {
                default:
                case Dil.Turkce:
                    ViewBag.Title = "Danışmanlık" + ((AnahtarDeger)titlesirketadi_OR.ReturnObject).Deger;
                    break;

                case Dil.Ingilizce:
                    ViewBag.Title = "Consultancy" + ((AnahtarDeger)titlesirketadi_OR.ReturnObject).Deger;
                    break;

            }

            ViewBag.Description = ((AnahtarDeger)description_OR.ReturnObject).Deger;
            ViewBag.MainKeywords = ((AnahtarDeger)mainkeywords_OR.ReturnObject).Deger;

            return View(anaSayfaViewModel);
        }
    }
}
