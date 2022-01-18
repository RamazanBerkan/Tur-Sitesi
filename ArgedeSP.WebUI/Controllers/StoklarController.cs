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
    public class StoklarController : Controller
    {
        private IHizmetBS _hizmetBS;
        private Dil SuankiDil;
        private IAnahtarDegerBS _anahtarDegerBS;

        public StoklarController(IHizmetBS hizmetBS, IAnahtarDegerBS anahtarDegerBS)
        {
            _hizmetBS = hizmetBS;
            _anahtarDegerBS = anahtarDegerBS;
            SuankiDil = CultureInfo.CurrentCulture.DilGetir();
        }

        public IActionResult Stoklar()
        {
            OperationResult hizmetler_OR = _hizmetBS.HizmetleriGetir(SuankiDil);
            OperationResult titlesirketadi_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.TitleSirketAdi);

            HizmetlerViewModel hizmetlerViewModel = new HizmetlerViewModel()
            {
                Hizmetler = (List<Hizmet>)hizmetler_OR.ReturnObject
            };
            switch (SuankiDil)
            {
                default:
                case Dil.Turkce:
                    ViewBag.Title = "Stoklar - " + ((AnahtarDeger)titlesirketadi_OR.ReturnObject).Deger;
                    break;

                case Dil.Ingilizce:
                    ViewBag.Title = "Stocks - " + ((AnahtarDeger)titlesirketadi_OR.ReturnObject).Deger;
                    break;

            }

            return View(hizmetlerViewModel);
        }
     
    }
}
