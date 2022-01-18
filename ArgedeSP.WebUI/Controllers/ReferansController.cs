using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Helpers;
using ArgedeSP.Contracts.Interfaces.BusinessLogicLayers;
using ArgedeSP.Contracts.Models.Common;
using ArgedeSP.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.WebUI.Controllers
{
    public class ReferansController : Controller
    {
        private Dil SuankiDil;
        private IReferansBS _refereansBS;
        private ISliderResimBS _sliderResimBS;


        public ReferansController( 
            IReferansBS referansBS, 
            ISliderResimBS sliderResimBS
            )
        {
            _refereansBS = referansBS;
            _sliderResimBS = sliderResimBS;


            SuankiDil = CultureInfo.CurrentCulture.DilGetir();

        }

        public async Task<IActionResult> Referanslar()
        {

            OperationResult sliderResimleri_OR = await _sliderResimBS.SliderBannerResimleriGetir(SuankiDil, "Referanslar");
            OperationResult referanslar_OR = await _refereansBS.ReferanslariGetir(SuankiDil);
            List<Referans> referanslar = (List<Referans>)referanslar_OR.ReturnObject;
            

            ReferansViewModel referansViewModel = new ReferansViewModel()
            {
                Referanslar = referanslar,
                SliderResim = (List<SliderResim>)sliderResimleri_OR.ReturnObject,


            };




            return View(referansViewModel);
        }

    }
}