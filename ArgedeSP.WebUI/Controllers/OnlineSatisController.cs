using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Helpers;
using ArgedeSP.Contracts.Interfaces.BusinessLogicLayers;
using ArgedeSP.Contracts.Models.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.WebUI.Controllers
{
    public class OnlineSatisController: Controller
    {
        private IAnahtarDegerBS _anahtarDegerBS;
        private Dil SuankiDil;

        public OnlineSatisController( IAnahtarDegerBS anahtarDegerBS)
        {

            _anahtarDegerBS = anahtarDegerBS;
            SuankiDil = CultureInfo.CurrentCulture.DilGetir();

        }
        public ActionResult OnlineSatis()
        {


            OperationResult description_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Description);
            OperationResult titlesirketadi_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.TitleSirketAdi);
            OperationResult mainkeywords_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.MainKeywords);

            switch (SuankiDil)
            {
                default:
                case Dil.Turkce:
                    ViewBag.Title = "Online Satış" + ((AnahtarDeger)titlesirketadi_OR.ReturnObject).Deger;
                    break;

                case Dil.Ingilizce:
                    ViewBag.Title = "Online Sales" + ((AnahtarDeger)titlesirketadi_OR.ReturnObject).Deger;
                    break;

            }
            
            ViewBag.Description = ((AnahtarDeger)description_OR.ReturnObject).Deger;
            ViewBag.MainKeywords = ((AnahtarDeger)mainkeywords_OR.ReturnObject).Deger;

            return View();
        }

        
    }
}
