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
    public class SorularController : Controller
    {

        private ISliderResimBS _sliderResimBS;
        private IAnahtarDegerBS _anahtarDegerBS;
        private Dil SuankiDil;

        public SorularController(ISliderResimBS sliderResimBS,
            IAnahtarDegerBS anahtarDegerBS)
        {

            _sliderResimBS = sliderResimBS;
            _anahtarDegerBS = anahtarDegerBS;

            SuankiDil = CultureInfo.CurrentCulture.DilGetir();
        }

        public async Task<IActionResult> Sorular()
        {

            OperationResult sliderResimleri_OR = await _sliderResimBS.SliderBannerResimleriGetir(SuankiDil, "Sorular");
            OperationResult sorular_OR = _anahtarDegerBS.AnahtarGetir(SuankiDil, Tanimlamalar.SorularSayfasi);
            ViewBag.Sorular = ((AnahtarDeger)sorular_OR.ReturnObject).Deger;


            ViewBag.banner = (List<SliderResim>)sliderResimleri_OR.ReturnObject;

            return View();
        }
    }
}