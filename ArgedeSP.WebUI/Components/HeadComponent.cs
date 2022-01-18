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
    public class HeadComponent : ViewComponent
    {
        public IAnahtarDegerBS _anahtarDegerBS;

        private Dil SuankiDil;


        public HeadComponent(IAnahtarDegerBS anahtarDegerBS)
        {
            _anahtarDegerBS = anahtarDegerBS;
            SuankiDil = CultureInfo.CurrentCulture.DilGetir();

        }


        public IViewComponentResult Invoke()
        {
            //Proje adı
            AnahtarDeger projeAdi = (AnahtarDeger)_anahtarDegerBS.AnahtarGetir(SuankiDil, Tanimlamalar.ProjeAdi).ReturnObject;
            TempData["proje-adi"] = projeAdi.Deger;


            //Sosyal medya hesapları
            AnahtarDeger instagram = (AnahtarDeger)_anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Instagram).ReturnObject;
            AnahtarDeger facebook = (AnahtarDeger)_anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Facebook).ReturnObject;
            AnahtarDeger twitter = (AnahtarDeger)_anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Twitter).ReturnObject;
            AnahtarDeger email = (AnahtarDeger)_anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Email).ReturnObject;
            TempData["instagram"] = instagram?.Deger;
            TempData["facebook"] = facebook?.Deger;
            TempData["twitter"] = twitter?.Deger;
            TempData["email"] = email?.Deger; 

            //Footer aciklama
            AnahtarDeger footerYazi = (AnahtarDeger)_anahtarDegerBS.AnahtarGetir(SuankiDil, Tanimlamalar.FooterYazi).ReturnObject;
            TempData["footer-yazi"] = footerYazi?.Deger;

            //Telefon
            AnahtarDeger telefon = (AnahtarDeger)_anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.TelefonNumarasi).ReturnObject;
            TempData["telefon"] = telefon?.Deger;

            //Faks
            AnahtarDeger faks= (AnahtarDeger)_anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Faks).ReturnObject;
            TempData["faks"] = faks?.Deger;

            //Adres
            AnahtarDeger adres = (AnahtarDeger)_anahtarDegerBS.AnahtarGetir(SuankiDil, Tanimlamalar.Adres).ReturnObject;
            TempData["adres"] = adres?.Deger;

            AnahtarDeger harita = (AnahtarDeger)_anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.GoogleMap).ReturnObject;
            TempData["google-map"] = harita?.Deger;

            //ViewBag.Title = "FiveN1";
            //ViewBag.Keywords = "FiveN1";
            //ViewBag.Description = "FiveN1";


            return View();
        }
    }
}
