using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Interfaces.BusinessLogicLayers;
using ArgedeSP.Contracts.Models.Common;
using ArgedeSP.Contracts.Models.DTO.Kulanici.Req;
using ArgedeSP.Contracts.Models.DTO.Kulanici.Res;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class KullanicilarController : Controller
    {
        private IKullaniciBS _kullaniciBS;
        private UserManager<Kullanici> _userManager;

        public KullanicilarController(
            IKullaniciBS kullaniciBS,
            UserManager<Kullanici> userManager
            )
        {
            _kullaniciBS = kullaniciBS;
            _userManager = userManager;
        }


        [HttpGet]
        public IActionResult Kullanicilar()
        {
            return View();
        }
        [HttpPost]
        public JsonResult KullanicilariGetir()
        {
            var dict = Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());

            var sayfaBoyutu = int.Parse(dict["length"]);
            var sayfa = int.Parse(dict["start"]);

            int sayfa2 = 0;
            if (sayfa == 0)
            {
                sayfa2 = 1;
            }
            else
            {
                sayfa2 = (sayfa / sayfaBoyutu) + 1;
            }


            KullaniciListele_REQ kullaniciListele_REQ = new KullaniciListele_REQ();


            string engelliMi_S = dict["columns[10][search][value]"];
            bool? engelliMi_N = null;
            if (!string.IsNullOrWhiteSpace(engelliMi_S))
            {
                bool engelliMi;
                if (bool.TryParse(engelliMi_S, out engelliMi))
                {
                    engelliMi_N = engelliMi;
                };
            }

            if (engelliMi_N.HasValue)
            {
                if (engelliMi_N.Value)
                {
                    kullaniciListele_REQ.Durum = Durum.Aktif;
                }
                else
                {
                    kullaniciListele_REQ.Durum = Durum.Pasif;

                }
            }

            kullaniciListele_REQ.Id = dict["columns[0][search][value]"];
            kullaniciListele_REQ.TCKN = dict["columns[2][search][value]"];
            kullaniciListele_REQ.Ad = dict["columns[3][search][value]"];
            kullaniciListele_REQ.PhoneNumber = dict["columns[4][search][value]"];
            kullaniciListele_REQ.Email = dict["columns[5][search][value]"];
            kullaniciListele_REQ.Universite = dict["columns[6][search][value]"];
            kullaniciListele_REQ.Fakulte = dict["columns[7][search][value]"];
            kullaniciListele_REQ.Sinif = dict["columns[8][search][value]"];
            kullaniciListele_REQ.OdaNumarasi = dict["columns[9][search][value]"];

            kullaniciListele_REQ.Sayfa = sayfa2;
            kullaniciListele_REQ.SayfaBoyutu = sayfaBoyutu;

            OperationResult veriListeleme_OR = _kullaniciBS.KullanicilariListele(kullaniciListele_REQ);

            if (veriListeleme_OR.IsSuccess)
            {
                VeriListeleme veriListeleme = (VeriListeleme)veriListeleme_OR.ReturnObject;

                return Json(new
                {
                    iTotalRecords = veriListeleme.ToplamVeri,
                    iTotalDisplayRecords = veriListeleme.ToplamVeri,
                    sEcho = 0,
                    sColumns = "",
                    aaData = ((IEnumerable<Kullanici>)veriListeleme.Veri).Select(x => new
                    {
                        x.Id,
                        x.UserName,
                        AdSoyad = x.AdSoyad(),
                        Durum = x.Durum == Durum.Aktif ? true : false,
                        x.Email,
                        x.PhoneNumber,
                        x.ProfilResmi,
                        x.TCKN,
                        OlusturmaTarihi = x.OlusturmaTarihi.ToString("dd.MM.yyyy")
                    })
                });
            }

            return Json(new
            {
                iTotalRecords = 0,
                iTotalDisplayRecords = 0,
                sEcho = 0,
                sColumns = "",
            });

        }

        [HttpGet]
        public IActionResult KullaniciEkle()
        {
            return View(new Kullanici_REQ()
            {
                Durum = true,
                ProfilGizliMi = true
            });
        }
        [HttpPost]
        public async Task<IActionResult> KullaniciEkle([FromForm]Kullanici_REQ kullanici_REQ)
        {
            if (!ModelState.IsValid)
            {
                return View(kullanici_REQ);
            }

            OperationResult kullaniciEkle_OR = await _kullaniciBS.KullaniciEkle(kullanici_REQ);

            if (!kullaniciEkle_OR.IsSuccess)
            {
                switch (kullaniciEkle_OR.Message)
                {
                    case MesajKodu.KullaniciEmailiZatenVar:
                        ModelState.AddModelError("Kullanici_REQ.Email", "Bu e-mail zaten kayıtlı");
                        break;
                    case MesajKodu.SifreZorunlu:
                        ModelState.AddModelError("Kullanici_REQ.Password", "Şifre zorunlu");
                        break;
                    case MesajKodu.BeklenmedikHata:
                        ModelState.AddModelError("Kullanici_REQ.KullaniciId", "Beklenmedik bir hata meydana");
                        break;
                }

                return View(kullanici_REQ);
            }

            TempData["Basarili"] = "Kullanıcı başarıyla kaydedildi";
            return RedirectToAction(nameof(KullaniciDuzenle), new { kullaniciId = kullaniciEkle_OR.ReturnObject.GetType().GetProperty("KullaniciId").GetValue(kullaniciEkle_OR.ReturnObject, null) });
        }
        [HttpGet]
        public async Task<IActionResult> KullaniciDuzenle(string kullaniciId)
        {
            OperationResult kullanici_OR = await _kullaniciBS.KullaniciGetirIdIle(kullaniciId);

            if (!kullanici_OR.IsSuccess)
            {
                TempData["Hata"] = "Kullanıcı bulunamadı";
                return RedirectToAction(nameof(Kullanicilar));
            }

            Kullanici kullanici = (Kullanici)kullanici_OR.ReturnObject;

            return View(new Kullanici_REQ()
            {
                Ad = kullanici.Ad,
                Durum = kullanici.Durum == Durum.Aktif ? true : false,
                Email = kullanici.Email,
                Id = kullanici.Id,
                Resim = kullanici.ProfilResmi,
                Soyad = kullanici.Soyad,
                TCKN = kullanici.TCKN,               
            });
        }
        [HttpPost]
        public async Task<IActionResult> KullaniciDuzenle([FromForm]Kullanici_REQ kullanici_REQ)
        {
            if (!ModelState.IsValid)
            {
                return View(kullanici_REQ);
            }

            OperationResult kullaniciEkle_OR = await _kullaniciBS.KullaniciGuncelle(kullanici_REQ);

            if (!kullaniciEkle_OR.IsSuccess)
            {
                switch (kullaniciEkle_OR.Message)
                {
                    case MesajKodu.KullaniciEmailiZatenVar:
                        ModelState.AddModelError("Kullanici_REQ.Email", "Bu e-mail zaten kayıtlı");
                        break;
                    case MesajKodu.BeklenmedikHata:
                        ModelState.AddModelError("Kullanici_REQ.KullaniciId", "Beklenmedik bir hata meydana");
                        break;
                }

                return View(kullanici_REQ);
            }

            TempData["Basarili"] = "Kullanıcı başarıyla kaydedildi";
            return RedirectToAction(nameof(KullaniciDuzenle), new { kullaniciId = kullanici_REQ.Id });

        }
    }
}