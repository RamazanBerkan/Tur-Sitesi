using ArgedeSP.BLL.BusinessServices;
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
    public class KategoriComponent : ViewComponent
    {
        public IAnahtarDegerBS _anahtarDegerBS;
        IUrunKategoriBS _urunKategoriBS;
        private Dil SuankiDil;


        public KategoriComponent(IAnahtarDegerBS anahtarDegerBS, IUrunKategoriBS urunKategoriBS)
        {
            _anahtarDegerBS = anahtarDegerBS;
            _urunKategoriBS = urunKategoriBS;
            SuankiDil = CultureInfo.CurrentCulture.DilGetir();

        }

        public IViewComponentResult Invoke()
        {

            OperationResult kategoriDil_OR = _urunKategoriBS.UrunKategorileriGetir((Dil)SuankiDil, int.MaxValue);
            List<UrunKategori> kategoriler = (List<UrunKategori>)kategoriDil_OR.ReturnObject;
            ViewBag.Kategoriler = kategoriler;

            return View();
        }





    }
}
