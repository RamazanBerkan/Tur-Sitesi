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

namespace ArgedeSP.WebUI.Components
{
    public class HeaderComponent : ViewComponent
    {
        public IAnahtarDegerBS _anahtarDegerBS;

        private Dil SuankiDil;


        public HeaderComponent(IAnahtarDegerBS anahtarDegerBS)
        {
            _anahtarDegerBS = anahtarDegerBS;
            SuankiDil = CultureInfo.CurrentCulture.DilGetir();

        }

        public IViewComponentResult Invoke()
        {
            AnahtarDeger telefonNo = (AnahtarDeger)_anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.TelefonNumarasi).ReturnObject;
            AnahtarDeger faks = (AnahtarDeger)_anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Faks).ReturnObject;
            AnahtarDeger email = (AnahtarDeger)_anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Email).ReturnObject;
            AnahtarDeger projeAdi= (AnahtarDeger)_anahtarDegerBS.AnahtarGetir(SuankiDil, Tanimlamalar.ProjeAdi).ReturnObject;

            ViewBag.TelefonNumarasi = telefonNo.Deger;
            ViewBag.Faks = faks.Deger;
            ViewBag.Email = email.Deger;
            ViewBag.ProjeAdi = projeAdi.Deger;
            return View();
        }
    }
}
