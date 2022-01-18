using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Helpers;
using ArgedeSP.Contracts.Interfaces.BusinessLogicLayers;
using ArgedeSP.Contracts.Models.Common;
using ArgedeSP.WebUI.Models;
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using static ArgedeSP.Contracts.Models.Common.Enums;
using Newtonsoft.Json;
using ArgedeSP.WebUI.Helpers;
using ArgedeSP.Contracts.Helpers.Mail;
using ArgedeSP.Contracts.Models.DTO.Iletisim.Req;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace ArgedeSP.WebUI.Controllers
{
    public class AnaSayfaController : Controller
    {

        private ISliderResimBS _sliderResimBS;
        private IBlogBS _blogBS;
        private IAnahtarDegerBS _anahtarDegerBS;
        private IHizmetBS _hizmetBS;
        private IOnHizmetBS _onhizmetBS;
        private IUrunBS _urunBS;
        private IUrunKategoriBS _urunKategoriBS;
        private IHaberBS _haberBS;

        private Dil SuankiDil;

        private readonly IRazorPartialToStringRenderer _renderer;
        private readonly IEmailSender2 _emailSender;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IBlogKategoriBS _blogKategoriBS;


        public AnaSayfaController(
            ISliderResimBS sliderResimBS,
            IBlogBS blogBS,
            IAnahtarDegerBS anahtarDegerBS,
            IHizmetBS hizmetBS,
            IOnHizmetBS onhizmetBS,
            IUrunBS urunBS,
            IUrunKategoriBS urunKategoriBS,
            IHaberBS haberBS,
             IRazorPartialToStringRenderer renderer,
            IEmailSender2 emailSender,
            IBlogKategoriBS blogKategoriBS,
            IStringLocalizer<SharedResources> localizer)
        {
            _sliderResimBS = sliderResimBS;
            _blogBS = blogBS;
            _anahtarDegerBS = anahtarDegerBS;
            _hizmetBS = hizmetBS;
            _onhizmetBS = onhizmetBS;
            _urunBS = urunBS;
            _urunKategoriBS = urunKategoriBS;
            _haberBS = haberBS;
            _blogKategoriBS = blogKategoriBS;
            _renderer = renderer;
            _emailSender = emailSender;
            _localizer = localizer;

            SuankiDil = Dil.Turkce;
        }
        public async Task<IActionResult> AnaSayfa()
        {
            OperationResult sliderResimleri_OR = await _sliderResimBS.SliderResimleriGetir(SuankiDil, SliderYeri.AnasayfaSlider);
            OperationResult bannerResimleri_OR = await _sliderResimBS.SliderResimleriGetir(SuankiDil, SliderYeri.AnasayfaBanner);
            OperationResult sonSekizBlog_OR = _blogBS.BloglariGetir(SuankiDil, 4);
            OperationResult projeAdi = _anahtarDegerBS.AnahtarGetir(SuankiDil, Tanimlamalar.ProjeAdi);
            OperationResult banner = _anahtarDegerBS.AnahtarGetir(SuankiDil, Tanimlamalar.AnaSayfaBanner);
            OperationResult urunler_OR = _urunBS.AktifUrunleriGetirAnsayfa(SuankiDil);
            OperationResult Blogkategoriler_QR = _blogKategoriBS.BlogKategorileriGetir(SuankiDil, 4);
            OperationResult haberler_OR = _haberBS.HaberleriGetir(SuankiDil,int.MaxValue);

            //OperationResult kategoriler_QR = _urunKategoriBS.UrunKategorileriGetir(SuankiDil, 4);
            OperationResult hakkimizda_OR = _anahtarDegerBS.AnahtarGetir(SuankiDil, Tanimlamalar.Hakkımızda2Sayfasi);
            ViewBag.Hakkimizda = ((AnahtarDeger)hakkimizda_OR.ReturnObject).Deger;
            //OperationResult hizmetler_OR = _hizmetBS.HizmetleriGetir(SuankiDil);
            //OperationResult onhizmetler_OR = _onhizmetBS.OnHizmetleriGetir(SuankiDil);

            AnaSayfaViewModel anaSayfaViewModel = new AnaSayfaViewModel()
            {
                BannerResim= ((List<SliderResim>)bannerResimleri_OR.ReturnObject).FirstOrDefault(),
                SliderResimleri = (List<SliderResim>)sliderResimleri_OR.ReturnObject,
                SonBloglar = (List<Blog>)sonSekizBlog_OR.ReturnObject,
                ProjeAdi = ((AnahtarDeger)projeAdi.ReturnObject).Deger,
                Urunler = (List<Urun>)urunler_OR.ReturnObject,
                AnaSayfaBanner = JsonConvert.DeserializeObject<AnaSayfaBanner>(((AnahtarDeger)banner.ReturnObject).Deger),
                BlogKategorileri= (List<BlogKategori>)Blogkategoriler_QR.ReturnObject,
                Haberler = (List<Haber>)haberler_OR.ReturnObject
                // Hizmetler = (List<Hizmet>)hizmetler_OR.ReturnObject,
                //OnHizmetler = (List<OnHizmet>)onhizmetler_OR.ReturnObject,
                //Kategoriler = (List<UrunKategori>)kategoriler_QR.ReturnObject,
            };
            OperationResult description_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Description);
            OperationResult titlesirketadi_OR = _anahtarDegerBS.AnahtarGetir(Dil.Turkce, Tanimlamalar.ProjeAdi);
            OperationResult mainkeywords_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.MainKeywords);


            switch(SuankiDil)
            {
                default:
                case Dil.Turkce:
                ViewBag.Title = "Anasayfa - " + ((AnahtarDeger)titlesirketadi_OR.ReturnObject).Deger;
                    break;

                case Dil.Ingilizce:
                    ViewBag.Title = "Home - " + ((AnahtarDeger)titlesirketadi_OR.ReturnObject).Deger;
                    break;
              
            }

            ViewBag.Description = ((AnahtarDeger)description_OR.ReturnObject).Deger;
            ViewBag.MainKeywords = ((AnahtarDeger)mainkeywords_OR.ReturnObject).Deger;

            return View(anaSayfaViewModel);
        }

        [HttpGet]
        public IActionResult SetLanguage(string culture)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return RedirectToAction("AnaSayfa", "AnaSayfa", new { area = "" });
        }


        /*İLETİŞİM FORMU*/
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AnaSayfa([FromForm] IletisimForm_REQ iletisimForm_REQ)
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

            return RedirectToAction("AnaSayfa");
        }

  

    }
}