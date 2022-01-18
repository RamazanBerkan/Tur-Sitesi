using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Interfaces.BusinessLogicLayers;
using ArgedeSP.Contracts.Models.Common;
using ArgedeSP.Contracts.Models.DTO.Haberler.Req;
using ArgedeSP.Contracts.Models.DTO.Haberler.Res;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HaberlerController : Controller
    {
        public IHaberBS _haberBS;
        public HaberlerController(IHaberBS haberBS)
        {
            _haberBS = haberBS;
        }
        [HttpGet]
        public IActionResult Haberler()
        {
            return View();
        }
        [HttpPost]
        public JsonResult HaberleriGetir()
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
            string haberAdi = dict["columns[1][search][value]"];
            int dil = 0;
            int.TryParse(dict["columns[3][search][value]"], out dil);


            OperationResult veriListeleme_OR = _haberBS.HaberleriSayfala(sayfa2, sayfaBoyutu, id, haberAdi, (Dil)dil);

            if (veriListeleme_OR.IsSuccess)
            {
                VeriListeleme veriListeleme = (VeriListeleme)veriListeleme_OR.ReturnObject;

                return Json(new
                {
                    iTotalRecords = veriListeleme.ToplamVeri,
                    iTotalDisplayRecords = veriListeleme.ToplamVeri,
                    sEcho = 0,
                    sColumns = "",
                    aaData = ((List<Haber>)veriListeleme.Veri).Select(x => new
                    {
                        x.Id,
                        Ad = x.Baslik,
                        x.Resim,
                        x.Dil,
                        OlusturmaTarihi = x.OlusturmaTarihi.ToString("f"),
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
        public IActionResult HaberEkle()
        {
            return View(new HaberEkle_RES()
            {
                HaberEkle_REQ = new HaberEkle_REQ()
            });
        }
        [HttpPost]
        public async Task<IActionResult> HaberEkle([FromForm]HaberEkle_REQ haberEkle_REQ)
        {
            if (!ModelState.IsValid)
            {
                return View(new HaberEkle_RES()
                {
                    HaberEkle_REQ = new HaberEkle_REQ()
                });
            }

            OperationResult haberEkle_OR = await _haberBS.HaberEkle(haberEkle_REQ);
            if (haberEkle_OR.IsSuccess)
            {
                TempData["Basarili"] = "Haber başarıyla eklendi";
                return RedirectToAction("HaberEkle");
            }

            TempData["Hata"] = "Haber eklerken beklenmedik bir hata meydana geldi.";
            return RedirectToAction("HaberEkle");


        }
        [HttpPost]
        public async Task<JsonResult> HaberSil(int haberId)
        {
            if (haberId == 0)
            {
                return Json(new Genel_RES()
                {
                    BasariliMi = false,
                    Mesaj = "Haber bulunamadı",
                });
            }

            OperationResult haberSil_OR = await _haberBS.HaberSil(haberId);

            if (haberSil_OR.IsSuccess)
            {
                return Json(new Genel_RES()
                {
                    BasariliMi = true,
                    Mesaj = "Haber başarıyla silindi."
                });
            }

            Genel_RES sonuc = new Genel_RES();
            sonuc.BasariliMi = false;

            switch (haberSil_OR.Message)
            {
                case MesajKodu.HizmetBulunamadi:
                    sonuc.Mesaj = "Haber bulunamadi";
                    break;
                default:
                    sonuc.Mesaj = "Beklenmedik bir hata meydana geldi";
                    break;
            }

            return Json(sonuc);
        }
        [HttpGet]
        public async Task<IActionResult> HaberDuzenle(int haberId)
        {
            if (haberId == 0)
            {
                TempData["Hata"] = "Hizmet bulunamadi";
                return RedirectToAction("Hizmetler");
            }

            OperationResult haber_OR = await _haberBS.HaberGetirIdIle(haberId);
            Haber haber = (Haber)haber_OR.ReturnObject;
            if (!haber_OR.IsSuccess || haber == null)
            {
                TempData["Hata"] = "Haber bulunamadi";
                return RedirectToAction("Haberler");
            }


            return View(new HaberEkle_RES()
            {
                HaberEkle_REQ = new HaberEkle_REQ()
                {
                    Baslik = haber.Baslik,
                    Id = haber.Id,
                    Resim = haber.Resim,
                    UzunAciklama = haber.UzunAciklama,
                    KisaAciklama = haber.KisaAciklama,
                    Dil = haber.Dil
                }
            });

        }
        [HttpPost]
        public async Task<IActionResult> HaberDuzenle([FromForm]HaberEkle_REQ haberEkle_REQ)
        {
            if (!ModelState.IsValid)
            {
                return View(new HaberEkle_RES()
                {
                    HaberEkle_REQ = haberEkle_REQ,
                });
            }

            if (haberEkle_REQ.Id == 0)
            {
                TempData["Hata"] = "Haber Bulunamadı";
                return RedirectToAction("Haberler");
            }

            OperationResult hizmetEkle_OR = await _haberBS.HaberGuncelle(haberEkle_REQ);
            if (hizmetEkle_OR.IsSuccess)
            {
                TempData["Basarili"] = "Haber başarıyla güncellendi";
                return RedirectToAction("HaberDuzenle", new { haberId = haberEkle_REQ.Id });
            }

            TempData["Hata"] = "Haberi güncellerken beklenmedik bir hata meydana geldi.";

            return View(new HaberEkle_RES()
            {
                HaberEkle_REQ = haberEkle_REQ,
            });
        }
    }
}