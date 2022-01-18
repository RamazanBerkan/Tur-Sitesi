using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Helpers;
using ArgedeSP.Contracts.Interfaces.BusinessLogicLayers;
using ArgedeSP.Contracts.Models.Common;
using ArgedeSP.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.WebUI.Controllers
{
    public class TravelController : Controller
    {
        private ISliderResimBS _sliderResimBS;
        private IUrunBS _urunBS;
        private IUrunKategoriBS _urunKategoriBS;
        private Dil SuankiDil;
        private IAnahtarDegerBS _anahtarDegerBS;

        public TravelController(IUrunBS urunBS, IUrunKategoriBS urunKategoriBS, ISliderResimBS sliderResimBS, IAnahtarDegerBS anahtarDegerBS)
        {
            _urunBS = urunBS;
            _urunKategoriBS = urunKategoriBS;
            _sliderResimBS = sliderResimBS;
            _anahtarDegerBS = anahtarDegerBS;
            SuankiDil = CultureInfo.CurrentCulture.DilGetir();

        }



        public async Task<IActionResult> TravelDetay(string urunSeoUrl)
        {



            OperationResult urun_OR = await _urunBS.UrunGetir(urunSeoUrl);
            if (!urun_OR.IsSuccess)
            {
                string hataMesaji = string.Empty;
                switch (urun_OR.Message)
                {
                    case MesajKodu.UrunBulunamadi:
                        hataMesaji = "Çözüm bulunamadı";
                        break;
                    case MesajKodu.BeklenmedikHata:
                        hataMesaji = "Beklenmedik bir hata meydana geldi";
                        break;
                }
                return RedirectToAction("Hata", "Hatalar", new { mesaj = hataMesaji });
            }

            Urun urun = (Urun)urun_OR.ReturnObject;
            if (urun.Durum == Durum.Pasif)
            {
                return RedirectToAction("Hata", "Hatalar", new { mesaj = "Ürün bulunamadı" });
            }


            OperationResult sliderResimleri_OR = await _sliderResimBS.SliderBannerResimleriGetir(SuankiDil, urun.UrunKategori.SeoUrl);

            TravelDetayViewModel travelDetayViewModel = new TravelDetayViewModel()
            {
                Urun = (Urun)urun_OR.ReturnObject,
                SizinIcinSectiklerimiz = ((List<Urun>)_urunBS.RandomKategoriGetir(4, SuankiDil, urun.UrunKategori.SeoUrl).ReturnObject).Where(x => x.Id != urun.Id).ToList(),
                SliderResim = (List<SliderResim>)sliderResimleri_OR.ReturnObject,
                UrunKategorileri = (List<UrunKategori>)(_urunKategoriBS.UrunKategorileriGetir(SuankiDil, int.MaxValue).ReturnObject)
            };

            ViewBag.Kategoriler = GetOncekiKategoriler(travelDetayViewModel);
            OperationResult description_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Description);
            OperationResult titlesirketadi_OR = _anahtarDegerBS.AnahtarGetir(Dil.Turkce, Tanimlamalar.ProjeAdi);


            ViewBag.Description = ((AnahtarDeger)description_OR.ReturnObject).Deger;

            ViewBag.Title = urun.UrunAdi + " - " + ((AnahtarDeger)titlesirketadi_OR.ReturnObject).Deger;
            ViewBag.MainKeywords = urun.Keywords;
           

            return View(travelDetayViewModel);



        }

        private List<UrunKategori> GetOncekiKategoriler(TravelDetayViewModel model)
        {
            List<UrunKategori> retVal = new List<UrunKategori>();
            UrunKategori kategori = model.Urun.UrunKategori;
            while (kategori != null)
            {
                retVal.Add(kategori);
                if (kategori.UstId == null) break;
                kategori = model.UrunKategorileri.Where(x => x.Id == kategori.UstId.Value).FirstOrDefault();
            }
            retVal.Reverse();
            return retVal;
        }

        public async Task<IActionResult> Travels(string anaKategoriAdi = "", string altKategoriAdi = "", string filtre = "")
        {

            OperationResult sliderResimleri_OR = await _sliderResimBS.SliderBannerResimleriGetir(SuankiDil, altKategoriAdi);

            OperationResult urunler_OR = _urunBS.AktifUrunleriGetir(SuankiDil, anaKategoriAdi, filtre);
            List<Urun> urunler = (List<Urun>)urunler_OR.ReturnObject;

            TravelViewModel urunViewModel = new TravelViewModel();
            urunViewModel.Urunler = urunler;
            urunViewModel.UrunKategorileri = (List<UrunKategori>)(_urunKategoriBS.UrunKategorileriGetir(SuankiDil, int.MaxValue).ReturnObject);
            urunViewModel.SliderResim = (List<SliderResim>)sliderResimleri_OR.ReturnObject;
            urunViewModel.altKategoriSeoUrl = altKategoriAdi;
            urunViewModel.filtre = filtre;
            urunViewModel.anaKategoriSeoUrl = anaKategoriAdi;
            urunViewModel.Urunler.Sort((x, y) => x.UrunSira.CompareTo(y.UrunSira));
            urunViewModel.NavbarResim = urunler.FirstOrDefault()?.UrunKategori.NavbarResim;

            OperationResult description_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Description);
            OperationResult titlesirketadi_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.TitleSirketAdi);

            switch (SuankiDil)
            {
                default:
                case Dil.Turkce:
                    ViewBag.Title = "Ürünlerimiz - " + ((AnahtarDeger)titlesirketadi_OR.ReturnObject).Deger;
                    break;

                case Dil.Ingilizce:
                    ViewBag.Title = "Products - " + ((AnahtarDeger)titlesirketadi_OR.ReturnObject).Deger;
                    break;

            }

            ViewBag.Description = ((AnahtarDeger)description_OR.ReturnObject).Deger;
            ViewBag.Kategori = urunler.FirstOrDefault()?.UrunKategori.Ad;

            return View(urunViewModel);
        }

    }
}
