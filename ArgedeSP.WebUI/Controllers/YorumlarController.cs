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
    public class YorumlarController : Controller
    {
        private IAnahtarDegerBS _anahtarDegerBS;
        private Dil SuankiDil;

        public YorumlarController(
            IAnahtarDegerBS anahtarDegerBS)
        {
            _anahtarDegerBS = anahtarDegerBS;

            SuankiDil = CultureInfo.CurrentCulture.DilGetir();
        }

        public IActionResult Yorumlar()
        {
            OperationResult yorumlar_OR = _anahtarDegerBS.AnahtarGetir(SuankiDil, Tanimlamalar.YorumlarSayfasi);
            if (yorumlar_OR.ReturnObject == null)
            {
                return View(new List<Yorum>());
            }
            List<Yorum> yorumlar = JsonConvert.DeserializeObject<List<Yorum>>(((AnahtarDeger)yorumlar_OR.ReturnObject).Deger);

            return View(yorumlar);
        }
    }
}