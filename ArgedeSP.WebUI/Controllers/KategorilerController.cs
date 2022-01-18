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
    public class KategorilerController : Controller
    {

        private Dil SuankiDil;
        private IUrunBS _urunBS;
        private IUrunKategoriBS _urunKategoriBS;
        private ISliderResimBS _sliderResimBS;
        private IAnahtarDegerBS _anahtarDegerBS;

        public KategorilerController(IUrunBS urunBS, IUrunKategoriBS urunKategoriBS, ISliderResimBS sliderResimBS, IAnahtarDegerBS anahtarDegerBS)
        {
            _urunBS = urunBS;
            _urunKategoriBS = urunKategoriBS;
            _sliderResimBS = sliderResimBS;
            _anahtarDegerBS = anahtarDegerBS;

            SuankiDil = CultureInfo.CurrentCulture.DilGetir();

        }

        public async Task<IActionResult> Kategoriler(string anaKategoriAdi = "", string altKategoriAdi = "", string filtre = "")
        {

            OperationResult sliderResimleri_OR = await _sliderResimBS.SliderBannerResimleriGetir(SuankiDil, altKategoriAdi);

            OperationResult urunler_OR = _urunBS.AktifUrunleriGetir(SuankiDil, altKategoriAdi, filtre);
            List<Urun> urunler = (List<Urun>)urunler_OR.ReturnObject;

            OperationResult kategoriDil_OR = _urunKategoriBS.UrunKategorileriGetir((Dil)SuankiDil, int.MaxValue);
            List<UrunKategori> kategoriler = (List<UrunKategori>)kategoriDil_OR.ReturnObject;
            ViewBag.Kategoriadi = kategoriler.Where(x => x.SeoUrl == altKategoriAdi).FirstOrDefault().Ad;
            ViewBag.Kategoriler = kategoriler.Where(x => x.UstKategori != null && x.UstKategori.SeoUrl == altKategoriAdi).ToList();
            ViewBag.NavbarResim = kategoriler.Where(x => x.SeoUrl == altKategoriAdi).FirstOrDefault().NavbarResim;


            OperationResult description_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Description);
            OperationResult titlesirketadi_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.TitleSirketAdi);
            OperationResult mainkeywords_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.MainKeywords);
            switch (SuankiDil)
            {
                default:
                case Dil.Turkce:
                    ViewBag.Title = "Kategoriler" + ((AnahtarDeger)titlesirketadi_OR.ReturnObject).Deger;
                    break;

                case Dil.Ingilizce:
                    ViewBag.Title = "Categories" + ((AnahtarDeger)titlesirketadi_OR.ReturnObject).Deger;
                    break;

            }

            ViewBag.Description = ((AnahtarDeger)description_OR.ReturnObject).Deger;
            ViewBag.MainKeywords = ((AnahtarDeger)mainkeywords_OR.ReturnObject).Deger;

            return View();
        }


    }
}
