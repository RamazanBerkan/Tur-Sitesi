using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Interfaces.BusinessLogicLayers;
using ArgedeSP.Contracts.Models.Common;
using ArgedeSP.Contracts.Models.DTO.Referanslar.Req;
using ArgedeSP.Contracts.Models.DTO.Referanslar.Res;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ReferanslarController : Controller
    {
        public IReferansBS _referansBS;
        public ReferanslarController(IReferansBS referansBS)
        {
            _referansBS = referansBS;
        }
        [HttpGet]
        public IActionResult Referanslar()
        {
            return View();
        }
        [HttpPost]
        public JsonResult ReferanslariGetir()
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
            int id = 0;
            bool id_bool = int.TryParse(dict["columns[0][search][value]"], out id);
            string ad = dict["columns[3][search][value]"];
            //string seoUrl = dict["columns[5][search][value]"];


            OperationResult veriListeleme_OR = _referansBS.ReferanslariSayfala(sayfa2, sayfaBoyutu, id, ad, "");

            if (veriListeleme_OR.IsSuccess)
            {
                VeriListeleme veriListeleme = (VeriListeleme)veriListeleme_OR.ReturnObject;

                return Json(new
                {
                    iTotalRecords = veriListeleme.ToplamVeri,
                    iTotalDisplayRecords = veriListeleme.ToplamVeri,
                    sEcho = 0,
                    sColumns = "",
                    aaData = ((List<Referans>)veriListeleme.Veri).Select(x => new
                    {
                        x.Id,
                        x.ReferansAdi,
                        x.Resim,
                        x.ArkaPlanResmi,
                        x.KisaAciklama,
                        x.SeoUrl
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
        public IActionResult ReferansEkle()
        {
            return View(new ReferansEkle_RES()
            {
                ReferansEkle_REQ = new ReferansEkle_REQ()
            });
        }
        [HttpPost]
        public async Task<IActionResult> ReferansEkle([FromForm]ReferansEkle_REQ referansEkle_REQ)
        {
            if (!ModelState.IsValid)
            {
                return View(new ReferansEkle_RES()
                {
                    ReferansEkle_REQ = referansEkle_REQ,
                });
            }

            OperationResult referansEkle_OR = await _referansBS.ReferansEkle(referansEkle_REQ);
            if (referansEkle_OR.IsSuccess)
            {
                TempData["Basarili"] = "Referans başarıyla eklendi";
                return RedirectToAction("ReferansEkle");
            }

            switch (referansEkle_OR.Message)
            {
                case MesajKodu.SeoUrlZatenVar:
                    ModelState.AddModelError("ReferansEkle_REQ.SeoUrl", "Bu seo url zaten kayıtlı, Lütfen değiştiriniz");
                    break;
                default:
                    TempData["Basarili"] = "Referans başarıyla eklendi";
                    return RedirectToAction("ReferansEkle");
            }

            return View(new ReferansEkle_RES()
            {
                ReferansEkle_REQ = referansEkle_REQ,
            });


        }
        [HttpPost]
        public async Task<JsonResult> ReferansSil(int referansId)
        {
            if (referansId == 0)
            {
                return Json(new Genel_RES()
                {
                    BasariliMi = false,
                    Mesaj = "Referans bulunamadı",
                });
            }

            OperationResult referansSil_OR = await _referansBS.ReferansSil(referansId);

            if (referansSil_OR.IsSuccess)
            {
                return Json(new Genel_RES()
                {
                    BasariliMi = true,
                    Mesaj = "Referans başarıyla silindi."
                });
            }

            Genel_RES sonuc = new Genel_RES();
            sonuc.BasariliMi = false;

            switch (referansSil_OR.Message)
            {
                case MesajKodu.ReferansBulunamadi:
                    sonuc.Mesaj = "Referans bulunamadi";
                    break;
                default:
                    sonuc.Mesaj = "Beklenmedik bir hata meydana geldi";
                    break;
            }

            return Json(sonuc);
        }
        [HttpGet]
        public async Task<IActionResult> ReferansDuzenle(int referansId)
        {
            if (referansId == 0)
            {
                TempData["Hata"] = "Referans bulunamadi";
                return RedirectToAction("Referanslar");
            }

            OperationResult referans_OR = await _referansBS.ReferanslariGetirIdIle(referansId);
            Referans referans = (Referans)referans_OR.ReturnObject;
            if (!referans_OR.IsSuccess || referans == null)
            {
                TempData["Hata"] = "Referans bulunamadi";
                return RedirectToAction("Referanslar");
            }

            return View(new ReferansEkle_RES()
            {
                ReferansEkle_REQ = new ReferansEkle_REQ()
                {
                    ReferansAdi = referans.ReferansAdi,
                    ArkaPlanResmi = referans.ArkaPlanResmi,
                    Id = referans.Id,
                    KisaAciklama = referans.KisaAciklama,
                    Resim = referans.Resim,
                    SeoUrl = referans.SeoUrl,
                    UzunAciklama = referans.UzunAciklama
                }
            });

        }
        [HttpPost]
        public async Task<IActionResult> ReferansDuzenle([FromForm]ReferansEkle_REQ referansEkle_REQ)
        {
             if (!ModelState.IsValid)
            {
                return View(new ReferansEkle_RES()
                {
                    ReferansEkle_REQ = referansEkle_REQ,
                });
            }

            if (referansEkle_REQ.Id == 0)
            {
                TempData["Hata"] = "Referans Bulunamadı";
                return RedirectToAction("Referanslar");
            }

            OperationResult referansEkle_OR = await _referansBS.ReferansGuncelle(referansEkle_REQ);
            if (referansEkle_OR.IsSuccess)
            {
                TempData["Basarili"] = "Referanslar başarıyla güncellendi";
                return RedirectToAction("ReferansDuzenle", new { referansId = referansEkle_REQ.Id });
            }

            switch (referansEkle_OR.Message)
            {
                case MesajKodu.SeoUrlZatenVar:
                    ModelState.AddModelError("ReferansEkle_REQ.SeoUrl", "Bu seo url zaten kayıtlı, Lütfen değiştiriniz");
                    break;
                case MesajKodu.ReferansGuncellendi:
                default:
                    TempData["Basarili"] = "Referans başarıyla güncellendi";
                    return RedirectToAction("ReferansDuzenle", new { referansId = referansEkle_REQ.Id });
            }

            return View(new ReferansEkle_RES()
            {
                ReferansEkle_REQ = referansEkle_REQ,
            });
        }
    }
}