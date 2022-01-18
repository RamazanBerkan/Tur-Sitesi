using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArgedeSP.Contracts.DTO.Hizmet.Req;
using ArgedeSP.Contracts.DTO.Hizmet.Res;
using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Interfaces.BusinessLogicLayers;
using ArgedeSP.Contracts.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HizmetlerController : Controller
    {
        public IHizmetBS _hizmetBS;
        public HizmetlerController(IHizmetBS hizmetBS)
        {
            _hizmetBS = hizmetBS;
        }
        [HttpGet]
        public IActionResult Hizmetler()
        {
            return View();
        }
        [HttpPost]
        public JsonResult HizmetleriGetir()
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
            string hizmetadi = dict["columns[1][search][value]"];
            string seoUrl = dict["columns[3][search][value]"];
            int dil = 0;
            int.TryParse(dict["columns[4][search][value]"], out dil);


            OperationResult veriListeleme_OR = _hizmetBS.HizmetleriSayfala(sayfa2, sayfaBoyutu, id, hizmetadi, seoUrl, (Dil)dil);

            if (veriListeleme_OR.IsSuccess)
            {
                VeriListeleme veriListeleme = (VeriListeleme)veriListeleme_OR.ReturnObject;

                return Json(new
                {
                    iTotalRecords = veriListeleme.ToplamVeri,
                    iTotalDisplayRecords = veriListeleme.ToplamVeri,
                    sEcho = 0,
                    sColumns = "",
                    aaData = ((List<Hizmet>)veriListeleme.Veri).Select(x => new
                    {
                        x.Id,
                        Ad = x.HizmetAdi,
                        x.Resim,
                        x.ArkaPlanResim,
                        x.Siralama,
                        x.Dil,
                        OlusturmaTarihi = x.OlusturmaTarihi.ToString("f"),
                        x.SeoUrl,
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
        public async Task<IActionResult> HizmetEkle()
        {
            return View(new HizmetEkle_RES()
            {
                HizmetEkle_REQ = new HizmetEkle_REQ(),
            });
        }

        [HttpPost]
        public async Task<IActionResult> HizmetEkle([FromForm]HizmetEkle_REQ hizmetEkle_REQ)
        {
            if (!ModelState.IsValid)
            {
                return View(new HizmetEkle_RES()
                {
                    HizmetEkle_REQ = hizmetEkle_REQ
                });
            }

            OperationResult hizmetEkle_OR = await _hizmetBS.HizmetEkle(hizmetEkle_REQ);
            if (hizmetEkle_OR.IsSuccess)
            {
                TempData["Basarili"] = "Stok başarıyla eklendi";
                return RedirectToAction("HizmetEkle");
            }

            switch (hizmetEkle_OR.Message)
            {
                case MesajKodu.SeoUrlZatenVar:
                    ModelState.AddModelError("HizmetEkle_REQ.SeoUrl", "Bu seo url zaten kayıtlı, Lütfen değiştiriniz");
                    break;
                default:
                    TempData["Basarili"] = "Stok başarıyla eklendi";
                    return RedirectToAction("HizmetEkle");
            }

            return View(new HizmetEkle_RES()
            {
                HizmetEkle_REQ = hizmetEkle_REQ,
            });


        }
        [HttpPost]
        public async Task<JsonResult> HizmetSil(int hizmetId)
        {
            if (hizmetId == 0)
            {
                return Json(new Genel_RES()
                {
                    BasariliMi = false,
                    Mesaj = "Stok bulunamadı",
                });
            }

            OperationResult hizmetSil_OR = await _hizmetBS.HizmetSil(hizmetId);

            if (hizmetSil_OR.IsSuccess)
            {
                return Json(new Genel_RES()
                {
                    BasariliMi = true,
                    Mesaj = "Stok başarıyla silindi."
                });
            }

            Genel_RES sonuc = new Genel_RES();
            sonuc.BasariliMi = false;

            switch (hizmetSil_OR.Message)
            {
                case MesajKodu.HizmetBulunamadi:
                    sonuc.Mesaj = "Stok bulunamadi";
                    break;
                default:
                    sonuc.Mesaj = "Beklenmedik bir hata meydana geldi";
                    break;
            }

            return Json(sonuc);
        }
        [HttpGet]
        public async Task<IActionResult> HizmetDuzenle(int hizmetId)
        {
            if (hizmetId == 0)
            {
                TempData["Hata"] = "Stok bulunamadi";
                return RedirectToAction("Hizmetler");
            }

            OperationResult hizmet_OR = await _hizmetBS.HizmetGetirIdIle(hizmetId);
            Hizmet hizmet = (Hizmet)hizmet_OR.ReturnObject;
            if (!hizmet_OR.IsSuccess || hizmet == null)
            {
                TempData["Hata"] = "Stok bulunamadi";
                return RedirectToAction("Hizmetler");
            }


            return View(new HizmetEkle_RES()
            {
                HizmetEkle_REQ = new HizmetEkle_REQ()
                {
                    HizmetAdi = hizmet.HizmetAdi,
                    Id = hizmet.Id,
                    Resim = hizmet.Resim,
                    SeoUrl = hizmet.SeoUrl,
                    Siralama=hizmet.Siralama,
                    ArkaplanResim=hizmet.ArkaPlanResim,
                    UzunAciklama = hizmet.UzunAciklama,
                    Dil = hizmet.Dil
                }
            });

        }
        [HttpPost]
        public async Task<IActionResult> HizmetDuzenle([FromForm]HizmetEkle_REQ hizmetEkle_REQ)
        {
            if (!ModelState.IsValid)
            {
                return View(new HizmetEkle_RES()
                {
                    HizmetEkle_REQ = hizmetEkle_REQ,
                });
            }

            if (hizmetEkle_REQ.Id == 0)
            {
                TempData["Hata"] = "Stok Bulunamadı";
                return RedirectToAction("Hizmetler");
            }

            OperationResult hizmetEkle_OR = await _hizmetBS.HizmetGuncelle(hizmetEkle_REQ);
            if (hizmetEkle_OR.IsSuccess)
            {
                TempData["Basarili"] = "Stok başarıyla güncellendi";
                return RedirectToAction("HizmetDuzenle", new { hizmetId = hizmetEkle_REQ.Id });
            }

            switch (hizmetEkle_OR.Message)
            {
                case MesajKodu.SeoUrlZatenVar:
                    ModelState.AddModelError("HizmetEkle_REQ.SeoUrl", "Bu seo url zaten kayıtlı, Lütfen değiştiriniz");
                    break;
                case MesajKodu.HizmetGuncellendi:
                default:
                    TempData["Basarili"] = "Stok başarıyla güncellendi";
                    return RedirectToAction("HizmetDuzenle", new { hizmetId = hizmetEkle_REQ.Id });
            }

            return View(new HizmetEkle_RES()
            {
                HizmetEkle_REQ = hizmetEkle_REQ,
            });
        }
    }
}
