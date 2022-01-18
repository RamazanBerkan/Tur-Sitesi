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

namespace Portemem.ViewComponents
{
    public class SlickComponent: ViewComponent
    {
        private IUrunBS _urunBS;
        private Dil SuankiDil;
        public SlickComponent(IUrunBS urunBS)
        {
            _urunBS = urunBS;

            SuankiDil = CultureInfo.CurrentCulture.DilGetir();

        }
        public IViewComponentResult Invoke()
        {

            OperationResult urunler_OR = _urunBS.AktifUrunleriGetir(SuankiDil, "", "");
            List<Urun> urunler = (List<Urun>)urunler_OR.ReturnObject;
            UrunViewModel urunViewModel = new UrunViewModel();
            urunViewModel.Urunler = urunler;
            return View(urunViewModel);
        }
    }
}

