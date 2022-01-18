using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Helpers;
using ArgedeSP.Contracts.Helpers.Extantions;
using ArgedeSP.Contracts.Helpers.Mail;
using ArgedeSP.Contracts.Interfaces.BusinessLogicLayers;
using ArgedeSP.Contracts.Models.Common;
using ArgedeSP.Contracts.Models.DTO.Iletisim.Req;
using ArgedeSP.WebUI.Helpers;
using ArgedeSP.WebUI.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.WebUI.Controllers
{
    public class IletisimController : Controller
    {
        private ISliderResimBS _sliderResimBS;
        private IAnahtarDegerBS _anahtarDegerBS;
        private readonly IRazorPartialToStringRenderer _renderer;
        private readonly IEmailSender2 _emailSender;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private Dil SuankiDil;

        public IletisimController(
            ISliderResimBS sliderResimBS,
            IAnahtarDegerBS anahtarDegerBS,
            IRazorPartialToStringRenderer renderer,
            IEmailSender2 emailSender,
            IStringLocalizer<SharedResources> localizer)
        {
            _sliderResimBS = sliderResimBS;
            _anahtarDegerBS = anahtarDegerBS;
            _renderer = renderer;
            _emailSender = emailSender;
            _localizer = localizer;
            SuankiDil = CultureInfo.CurrentCulture.DilGetir();
        }

        public async Task<IActionResult> Iletisim()
        {

            OperationResult sliderResimleri_OR = await _sliderResimBS.SliderBannerResimleriGetir(SuankiDil, "Iletisim");
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
                SliderResim = (List<SliderResim>)sliderResimleri_OR.ReturnObject,
                IletisimForm_REQ = new IletisimForm_REQ()

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

            return View(iletisimViewModel);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Iletisim([FromForm]IletisimForm_REQ iletisimForm_REQ)
        {
            OperationResult anahtarDeger_Mail_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Email);
            OperationResult anahtarDeger_ProjeAdi_OR = _anahtarDegerBS.AnahtarGetir(Dil.Turkce, Tanimlamalar.ProjeAdi);
            OperationResult anahtarDeger_SiteAdi_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.SiteAdi);


            string siteMail = ((AnahtarDeger)anahtarDeger_Mail_OR.ReturnObject).Deger;
            string projeAdi = ((AnahtarDeger)anahtarDeger_ProjeAdi_OR.ReturnObject).Deger;
            string siteAdi = ((AnahtarDeger)anahtarDeger_SiteAdi_OR.ReturnObject).Deger;

            var emailHtml = await _renderer.RenderPartialToStringAsync("~/Views/Iletisim/_IletisimFormu_EMAIL.cshtml", new IletisimViewModel()
            {
                ProjeAdi = projeAdi,
                SiteAdi = siteAdi,
                IletisimForm_REQ = iletisimForm_REQ
            });


            await _emailSender.SendEmailAsync(
                 email: siteMail,
                 subject: "İletişim Formu",
                 htmlMessage: emailHtml,
                 projectName: projeAdi);

            return RedirectToAction("Iletisim");
        }

        public IActionResult TalepFormu()
        {
            IletisimViewModel iletisimViewModel = new IletisimViewModel()
            {
                IletisimForm_REQ = new IletisimForm_REQ()

            };
            return View(iletisimViewModel);
        }
    }
}