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
    public class HaberlerController : Controller
    {
        private IAnahtarDegerBS _anahtarDegerBS;
        private IHaberBS _haberBS;
        private Dil SuankiDil;

        public HaberlerController(
            IAnahtarDegerBS anahtarDegerBS,
            IHaberBS haberBS)
        {
            _anahtarDegerBS = anahtarDegerBS;
            _haberBS = haberBS;

            SuankiDil = CultureInfo.CurrentCulture.DilGetir();
        }

        public IActionResult Haberler()
        {
            OperationResult haberler_OR = _haberBS.HaberleriGetir(SuankiDil, int.MaxValue);
            return View((List<Haber>)haberler_OR.ReturnObject);
        }
        public async Task<IActionResult> HaberDetayi(int haberId)
        {
            OperationResult haber_OR = await _haberBS.HaberGetirIdIle(haberId);
            return View((Haber)haber_OR.ReturnObject);
        }
    }
}