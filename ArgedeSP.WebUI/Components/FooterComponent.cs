using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Helpers;
using ArgedeSP.Contracts.Helpers.Mail;
using ArgedeSP.Contracts.Interfaces.BusinessLogicLayers;
using ArgedeSP.Contracts.Models.Common;
using ArgedeSP.WebUI.Helpers;
using ArgedeSP.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.WebUI.Components
{
    public class FooterComponent : ViewComponent
    {
        private IReferansBS _refereansBS;
        private Dil SuankiDil;
        private ISliderResimBS _sliderResimBS;
        private IAnahtarDegerBS _anahtarDegerBS;
        private readonly IRazorPartialToStringRenderer _renderer;
        private readonly IEmailSender2 _emailSender;
        private readonly IStringLocalizer<SharedResources> _localizer;
        

        public FooterComponent(IReferansBS referansBS,
         
            IAnahtarDegerBS anahtarDegerBS,
            IRazorPartialToStringRenderer renderer,
            IEmailSender2 emailSender,
            IStringLocalizer<SharedResources> localizer
            )
        {
            _refereansBS = referansBS;
            
            _anahtarDegerBS = anahtarDegerBS;
            _renderer = renderer;
            _emailSender = emailSender;
            _localizer = localizer;
            SuankiDil = CultureInfo.CurrentCulture.DilGetir();





        }

        public IViewComponentResult Invoke()
        {
            OperationResult referanslar_OR = _refereansBS.ReferanslariGetir(SuankiDil).Result;
            List<Referans> referanslar = (List<Referans>)referanslar_OR.ReturnObject;

            OperationResult adres_OR = _anahtarDegerBS.AnahtarGetir(SuankiDil, Tanimlamalar.Adres);
            OperationResult telefon_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.TelefonNumarasi);
            OperationResult mail_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Email);
            OperationResult fax_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Faks);
            OperationResult iletisimAciklamaBaslik_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.IletisimAciklamaBaslik);
            OperationResult iletisimAciklama_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.IletisimAciklama);

            OperationResult googleMap_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.GoogleMap);

            IletisimViewModel iletisimViewModel = new IletisimViewModel()
            {
                Adres = ((AnahtarDeger)adres_OR.ReturnObject).Deger,
                Email = ((AnahtarDeger)mail_OR.ReturnObject).Deger,
                TelefonNo = ((AnahtarDeger)telefon_OR.ReturnObject).Deger,
                Fax = ((AnahtarDeger)fax_OR.ReturnObject).Deger,
                IletisimAciklama = ((AnahtarDeger)iletisimAciklama_OR.ReturnObject).Deger,
                IletisimAciklamaBaslik = ((AnahtarDeger)iletisimAciklamaBaslik_OR.ReturnObject).Deger,
                GoogleMap = ((AnahtarDeger)googleMap_OR.ReturnObject).Deger,
              

            };
            OperationResult description_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Description);
            OperationResult titlesirketadi_OR = _anahtarDegerBS.AnahtarGetir(Dil.Turkce, Tanimlamalar.ProjeAdi);
            OperationResult mainkeywords_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.MainKeywords);

            switch (SuankiDil)
            {
                default:
                case Dil.Turkce:
                    ViewBag.Title = "İletişim -" + ((AnahtarDeger)titlesirketadi_OR.ReturnObject).Deger;
                    break;

                case Dil.Ingilizce:
                    ViewBag.Title = "Contact -" + ((AnahtarDeger)titlesirketadi_OR.ReturnObject).Deger;
                    break;

            }


            ViewBag.Description = ((AnahtarDeger)description_OR.ReturnObject).Deger;
            ViewBag.MainKeywords = ((AnahtarDeger)mainkeywords_OR.ReturnObject).Deger;



            ViewBag.referans = referanslar;
            return View(iletisimViewModel);
        }
        }
}
