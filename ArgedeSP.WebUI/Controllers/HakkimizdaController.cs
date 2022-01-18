using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Helpers;
using ArgedeSP.Contracts.Interfaces.BusinessLogicLayers;
using ArgedeSP.Contracts.Models.Common;
using Microsoft.AspNetCore.Mvc;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.WebUI.Controllers
{
    public class HakkimizdaController : Controller
    {

        private ISliderResimBS _sliderResimBS;
        private IAnahtarDegerBS _anahtarDegerBS;
        private Dil SuankiDil;

        public HakkimizdaController(ISliderResimBS sliderResimBS,
            IAnahtarDegerBS anahtarDegerBS)
        {

            _sliderResimBS = sliderResimBS;
            _anahtarDegerBS = anahtarDegerBS;

            SuankiDil = CultureInfo.CurrentCulture.DilGetir();
        }

        public async Task<IActionResult> Hakkimizda()
        {

            //OperationResult sliderResimleri_OR = await _sliderResimBS.SliderBannerResimleriGetir(SuankiDil, "Hakkimizda");
            OperationResult bannerResimleri_OR = await _sliderResimBS.SliderResimleriGetir(SuankiDil, SliderYeri.AnasayfaBanner);
            OperationResult hakkimizda_OR = _anahtarDegerBS.AnahtarGetir(SuankiDil, Tanimlamalar.HakkimizdaSayfasi);
            ViewBag.Hakkimizda = ((AnahtarDeger)hakkimizda_OR.ReturnObject).Deger;

            OperationResult hakkimizda1_OR = _anahtarDegerBS.AnahtarGetir(SuankiDil, Tanimlamalar.Hakkımızda1Sayfasi);
            ViewBag.Hakkimizda1 = ((AnahtarDeger)hakkimizda1_OR.ReturnObject).Deger;

            OperationResult hakkimizda2_OR = _anahtarDegerBS.AnahtarGetir(SuankiDil, Tanimlamalar.Hakkımızda2Sayfasi);
            ViewBag.Hakkimizda2 = ((AnahtarDeger)hakkimizda2_OR.ReturnObject).Deger;

            OperationResult hedeef_OR = _anahtarDegerBS.AnahtarGetir(SuankiDil, Tanimlamalar.HedefSayfasi);
            ViewBag.Hedef = ((AnahtarDeger)hedeef_OR.ReturnObject).Deger;

            OperationResult vizyonf_OR = _anahtarDegerBS.AnahtarGetir(SuankiDil, Tanimlamalar.VizyonMisyonSayfasi);
            ViewBag.Vizyon = ((AnahtarDeger)vizyonf_OR.ReturnObject).Deger;

            ViewBag.banner = (List<SliderResim>)bannerResimleri_OR.ReturnObject;

            //ViewBag.banner = (List<SliderResim>)sliderResimleri_OR.ReturnObject;


            OperationResult description_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Description);
            OperationResult titlesirketadi_OR = _anahtarDegerBS.AnahtarGetir(Dil.Turkce, Tanimlamalar.ProjeAdi);
            OperationResult mainkeywords_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.MainKeywords);
            ViewBag.Description = ((AnahtarDeger)description_OR.ReturnObject).Deger;

            switch (SuankiDil)
            {
                default:
                case Dil.Turkce:
                    ViewBag.Title = "Hakkımızda - " + ((AnahtarDeger)titlesirketadi_OR.ReturnObject).Deger;
                    break;

                case Dil.Ingilizce:
                    ViewBag.Title = "About Us - " + ((AnahtarDeger)titlesirketadi_OR.ReturnObject).Deger;
                    break;

            }
         
            ViewBag.MainKeywords = ((AnahtarDeger)mainkeywords_OR.ReturnObject).Deger;

            return View();
        }


        public async Task<IActionResult> GenelBakis()
        {

            OperationResult sliderResimleri_OR = await _sliderResimBS.SliderBannerResimleriGetir(SuankiDil, "GenelBakis");
            OperationResult hakkimizda_OR = _anahtarDegerBS.AnahtarGetir(SuankiDil, Tanimlamalar.GenelbakisSayfasi);
            ViewBag.Hakkimizda = ((AnahtarDeger)hakkimizda_OR.ReturnObject).Deger;


            ViewBag.banner = (List<SliderResim>)sliderResimleri_OR.ReturnObject;


            OperationResult description_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Description);
            OperationResult titlesirketadi_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.TitleSirketAdi);
            OperationResult mainkeywords_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.MainKeywords);
            ViewBag.Description = ((AnahtarDeger)description_OR.ReturnObject).Deger;

            switch (SuankiDil)
            {
                default:
                case Dil.Turkce:
                    ViewBag.Title = "Genel Bakış" + ((AnahtarDeger)titlesirketadi_OR.ReturnObject).Deger;
                    break;

            }

            ViewBag.MainKeywords = ((AnahtarDeger)mainkeywords_OR.ReturnObject).Deger;

            return View();
        }

        public async Task<IActionResult> Degerlerimiz()
        {

            OperationResult sliderResimleri_OR = await _sliderResimBS.SliderBannerResimleriGetir(SuankiDil, "Degerlerimiz");
            OperationResult hakkimizda_OR = _anahtarDegerBS.AnahtarGetir(SuankiDil, Tanimlamalar.DegerlerimizSayfasi);
            ViewBag.Degerlerimiz = ((AnahtarDeger)hakkimizda_OR.ReturnObject).Deger;


            ViewBag.banner = (List<SliderResim>)sliderResimleri_OR.ReturnObject;


            OperationResult description_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Description);
            OperationResult titlesirketadi_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.TitleSirketAdi);
            OperationResult mainkeywords_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.MainKeywords);
            ViewBag.Description = ((AnahtarDeger)description_OR.ReturnObject).Deger;

            switch (SuankiDil)
            {
                default:
                case Dil.Turkce:
                    ViewBag.Title = "Değerlerimiz" + ((AnahtarDeger)titlesirketadi_OR.ReturnObject).Deger;
                    break;

            }

            ViewBag.MainKeywords = ((AnahtarDeger)mainkeywords_OR.ReturnObject).Deger;

            return View();
        }

        public async Task<IActionResult> VizyonMisyon()
        {

            OperationResult sliderResimleri_OR = await _sliderResimBS.SliderBannerResimleriGetir(SuankiDil, "VizyonMisyon");
            OperationResult hakkimizda_OR = _anahtarDegerBS.AnahtarGetir(SuankiDil, Tanimlamalar.VisyonVeMisyonSayfasi);
            ViewBag.VizyonMisyon = ((AnahtarDeger)hakkimizda_OR.ReturnObject).Deger;


            ViewBag.banner = (List<SliderResim>)sliderResimleri_OR.ReturnObject;


            OperationResult description_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Description);
            OperationResult titlesirketadi_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.TitleSirketAdi);
            OperationResult mainkeywords_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.MainKeywords);
            ViewBag.Description = ((AnahtarDeger)description_OR.ReturnObject).Deger;

            switch (SuankiDil)
            {
                default:
                case Dil.Turkce:
                    ViewBag.Title = "Vizyon ve Misyon - " + ((AnahtarDeger)titlesirketadi_OR.ReturnObject).Deger;
                    break;

            }

            ViewBag.MainKeywords = ((AnahtarDeger)mainkeywords_OR.ReturnObject).Deger;

            return View();
        }

        public async Task<IActionResult> BaskaninMesaji()
        {

            OperationResult sliderResimleri_OR = await _sliderResimBS.SliderBannerResimleriGetir(SuankiDil, "BaskaninMesaji");
            OperationResult hakkimizda_OR = _anahtarDegerBS.AnahtarGetir(SuankiDil, Tanimlamalar.BaskaninMesajiSayfasi);
            ViewBag.BaskaninMesaji = ((AnahtarDeger)hakkimizda_OR.ReturnObject).Deger;


            ViewBag.banner = (List<SliderResim>)sliderResimleri_OR.ReturnObject;


            OperationResult description_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Description);
            OperationResult titlesirketadi_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.TitleSirketAdi);
            OperationResult mainkeywords_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.MainKeywords);
            ViewBag.Description = ((AnahtarDeger)description_OR.ReturnObject).Deger;

            switch (SuankiDil)
            {
                default:
                case Dil.Turkce:
                    ViewBag.Title = "Başkanın Mesajı" + ((AnahtarDeger)titlesirketadi_OR.ReturnObject).Deger;
                    break;

            }

            ViewBag.MainKeywords = ((AnahtarDeger)mainkeywords_OR.ReturnObject).Deger;

            return View();
        }

        public async Task<IActionResult> Yonetim()
        {

            OperationResult sliderResimleri_OR = await _sliderResimBS.SliderBannerResimleriGetir(SuankiDil, "Yonetim");
            OperationResult hakkimizda_OR = _anahtarDegerBS.AnahtarGetir(SuankiDil, Tanimlamalar.YonetimSayfasi);
            ViewBag.Yonetim = ((AnahtarDeger)hakkimizda_OR.ReturnObject).Deger;


            ViewBag.banner = (List<SliderResim>)sliderResimleri_OR.ReturnObject;


            OperationResult description_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Description);
            OperationResult titlesirketadi_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.TitleSirketAdi);
            OperationResult mainkeywords_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.MainKeywords);
            ViewBag.Description = ((AnahtarDeger)description_OR.ReturnObject).Deger;

            switch (SuankiDil)
            {
                default:
                case Dil.Turkce:
                    ViewBag.Title = "Yönetim" + ((AnahtarDeger)titlesirketadi_OR.ReturnObject).Deger;
                    break;

            }

            ViewBag.MainKeywords = ((AnahtarDeger)mainkeywords_OR.ReturnObject).Deger;

            return View();
        }

        public async Task<IActionResult> İnsanKaynaklari()
        {

            OperationResult sliderResimleri_OR = await _sliderResimBS.SliderBannerResimleriGetir(SuankiDil, "İnsanKaynaklari");
            OperationResult hakkimizda_OR = _anahtarDegerBS.AnahtarGetir(SuankiDil, Tanimlamalar.YonetimSayfasi);
            ViewBag.İnsanKaynaklari = ((AnahtarDeger)hakkimizda_OR.ReturnObject).Deger;


            ViewBag.banner = (List<SliderResim>)sliderResimleri_OR.ReturnObject;


            OperationResult description_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Description);
            OperationResult titlesirketadi_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.TitleSirketAdi);
            OperationResult mainkeywords_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.MainKeywords);
            ViewBag.Description = ((AnahtarDeger)description_OR.ReturnObject).Deger;

            switch (SuankiDil)
            {
                default:
                case Dil.Turkce:
                    ViewBag.Title = "İnsan Kaynakları" + ((AnahtarDeger)titlesirketadi_OR.ReturnObject).Deger;
                    break;

            }

            ViewBag.MainKeywords = ((AnahtarDeger)mainkeywords_OR.ReturnObject).Deger;

            return View();
        }
    }
}