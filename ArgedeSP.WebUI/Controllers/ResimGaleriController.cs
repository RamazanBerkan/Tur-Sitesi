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
using Newtonsoft.Json;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.WebUI.Controllers
{
    public class ResimGaleriController : Controller
    {
        private IAnahtarDegerBS _anahtarDegerBS;
        private IUrunKategoriBS _urunKategoriBS;
        private Dil SuankiDil;

        public ResimGaleriController(
            IAnahtarDegerBS anahtarDegerBS,
            IUrunKategoriBS urunKategoriBS)
        {
            _anahtarDegerBS = anahtarDegerBS;
            _urunKategoriBS = urunKategoriBS;
            SuankiDil = CultureInfo.CurrentCulture.DilGetir();
        }


        public IActionResult ResimGaleri()
        {
            OperationResult resimGalerisi_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.ResimGalerisi);
            List<ResimGalerisi> resimGalerisi = JsonConvert.DeserializeObject<List<ResimGalerisi>>(((AnahtarDeger)resimGalerisi_OR.ReturnObject).Deger);
            ViewBag.UrunKategorileri = _urunKategoriBS.UrunKategorileriGetir(SuankiDil,0).ReturnObject;



            OperationResult description_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Description);
            OperationResult titlesirketadi_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.TitleSirketAdi);
            OperationResult mainkeywords_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.MainKeywords);
            switch (SuankiDil)
            {
                default:
                case Dil.Turkce:
                    ViewBag.Title = "Galeri" + ((AnahtarDeger)titlesirketadi_OR.ReturnObject).Deger;
                    break;

                case Dil.Ingilizce:
                    ViewBag.Title = "Gallery" + ((AnahtarDeger)titlesirketadi_OR.ReturnObject).Deger;
                    break;

            }
        
            ViewBag.Description = ((AnahtarDeger)description_OR.ReturnObject).Deger;
            ViewBag.MainKeywords = ((AnahtarDeger)mainkeywords_OR.ReturnObject).Deger;

            return View(resimGalerisi);
        }
    }
}