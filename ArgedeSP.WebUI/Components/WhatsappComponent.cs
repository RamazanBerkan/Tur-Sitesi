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
    public class WhatsappComponent : ViewComponent
    {
        private Dil SuankiDil;
        private IAnahtarDegerBS _anahtarDegerBS;


        public WhatsappComponent(
            IAnahtarDegerBS anahtarDegerBS)
        {

            SuankiDil = CultureInfo.CurrentCulture.DilGetir();
            _anahtarDegerBS = anahtarDegerBS;
        }
        public IViewComponentResult Invoke()
        {

            OperationResult telefon_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.TelefonNumarasi);

            ViewBag.TelefonNo = ((AnahtarDeger)telefon_OR.ReturnObject).Deger;
            return View();
        }
    }
}
