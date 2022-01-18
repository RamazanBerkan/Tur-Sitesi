using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArgedeSP.Contracts.DTO.OnHizmet.Req;
using ArgedeSP.Contracts.DTO.OnHizmet.Res;
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
    public class OnHizmetlerController : Controller
    {
        public IOnHizmetBS _OnhizmetBS;
        public OnHizmetlerController(IOnHizmetBS OnhizmetBS)
        {
            _OnhizmetBS = OnhizmetBS;
        }
        [HttpGet]
        public IActionResult OnHizmetler()
        {
            return View();
        }
        [HttpPost]
        public JsonResult OnHizmetleriGetir()
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


            OperationResult veriListeleme_OR = _OnhizmetBS.OnHizmetleriSayfala(sayfa2, sayfaBoyutu, id, hizmetadi, seoUrl, (Dil)dil);

            if (veriListeleme_OR.IsSuccess)
            {
                VeriListeleme veriListeleme = (VeriListeleme)veriListeleme_OR.ReturnObject;

                return Json(new
                {
                    iTotalRecords = veriListeleme.ToplamVeri,
                    iTotalDisplayRecords = veriListeleme.ToplamVeri,
                    sEcho = 0,
                    sColumns = "",
                    aaData = ((List<OnHizmet>)veriListeleme.Veri).Select(x => new
                    {
                        x.Id,
                        Ad = x.OnHizmetAdi,
                        x.Resim,
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
        public IActionResult OnHizmetEkle()
        {
            return View(new OnHizmetEkle_RES()
            {
                OnHizmetEkle_REQ = new OnHizmetEkle_REQ(),
            });
        }
        [HttpPost]
        public async Task<IActionResult> OnHizmetEkle([FromForm]OnHizmetEkle_REQ OnhizmetEkle_REQ)
        {
            if (!ModelState.IsValid)
            {
                return View(new OnHizmetEkle_RES()
                {
                    OnHizmetEkle_REQ = OnhizmetEkle_REQ
                });
            }

            OperationResult onhizmetEkle_OR = await _OnhizmetBS.OnHizmetEkle(OnhizmetEkle_REQ);
            if (onhizmetEkle_OR.IsSuccess)
            {
                TempData["Basarili"] = "Hizmet başarıyla eklendi";
                return RedirectToAction("OnHizmetEkle");
            }

            switch (onhizmetEkle_OR.Message)
            {
                case MesajKodu.SeoUrlZatenVar:
                    ModelState.AddModelError("HizmetEkle_REQ.SeoUrl", "Bu seo url zaten kayıtlı, Lütfen değiştiriniz");
                    break;
                default:
                    TempData["Basarili"] = "Hizmet başarıyla eklendi";
                    return RedirectToAction("OnHizmetEkle");
            }

            return View(new OnHizmetEkle_RES()
            {
                OnHizmetEkle_REQ = OnhizmetEkle_REQ,
            });


        }
        [HttpPost]
        public async Task<JsonResult> OnHizmetSil(int onhizmetId)
        {
            if (onhizmetId == 0)
            {
                return Json(new Genel_RES()
                {
                    BasariliMi = false,
                    Mesaj = "Hizmet bulunamadı",
                });
            }

            OperationResult onhizmetSil_OR = await _OnhizmetBS.OnHizmetSil(onhizmetId);

            if (onhizmetSil_OR.IsSuccess)
            {
                return Json(new Genel_RES()
                {
                    BasariliMi = true,
                    Mesaj = "Hizmet başarıyla silindi."
                });
            }

            Genel_RES sonuc = new Genel_RES();
            sonuc.BasariliMi = false;

            switch (onhizmetSil_OR.Message)
            {
                case MesajKodu.HizmetBulunamadi:
                    sonuc.Mesaj = "Hizmet bulunamadi";
                    break;
                default:
                    sonuc.Mesaj = "Beklenmedik bir hata meydana geldi";
                    break;
            }

            return Json(sonuc);
        }
        [HttpGet]
        public async Task<IActionResult> OnHizmetDuzenle(int onhizmetId)
        {
            if (onhizmetId == 0)
            {
                TempData["Hata"] = "Hizmet bulunamadi";
                return RedirectToAction("OnHizmetler");
            }

            OperationResult onhizmet_OR = await _OnhizmetBS.OnHizmetGetirIdIle(onhizmetId);
            OnHizmet onhizmet = (OnHizmet)onhizmet_OR.ReturnObject;
            if (!onhizmet_OR.IsSuccess || onhizmet == null)
            {
                TempData["Hata"] = "Hizmet bulunamadi";
                return RedirectToAction("OnHizmetler");
            }


            return View(new OnHizmetEkle_RES()
            {
                OnHizmetEkle_REQ = new OnHizmetEkle_REQ()
                {
                    OnHizmetAdi = onhizmet.OnHizmetAdi,
                    Id = onhizmet.Id,
                    Resim = onhizmet.Resim,
                    SeoUrl = onhizmet.SeoUrl,
                    UzunAciklama = onhizmet.UzunAciklama,
                    Dil = onhizmet.Dil
                }
            });

        }
        [HttpPost]
        public async Task<IActionResult> OnHizmetDuzenle([FromForm]OnHizmetEkle_REQ onhizmetEkle_REQ)
        {
            if (!ModelState.IsValid)
            {
                return View(new OnHizmetEkle_RES()
                {
                    OnHizmetEkle_REQ = onhizmetEkle_REQ,
                });
            }

            if (onhizmetEkle_REQ.Id == 0)
            {
                TempData["Hata"] = "Hizmet Bulunamadı";
                return RedirectToAction("OnHizmetler");
            }

            OperationResult onhizmetEkle_OR = await _OnhizmetBS.OnHizmetGuncelle(onhizmetEkle_REQ);
            if (onhizmetEkle_OR.IsSuccess)
            {
                TempData["Basarili"] = "Hizmet başarıyla güncellendi";
                return RedirectToAction("OnHizmetDuzenle", new { onhizmetId = onhizmetEkle_REQ.Id });
            }

            switch (onhizmetEkle_OR.Message)
            {
                case MesajKodu.SeoUrlZatenVar:
                    ModelState.AddModelError("OnHizmetEkle_REQ.SeoUrl", "Bu seo url zaten kayıtlı, Lütfen değiştiriniz");
                    break;
                case MesajKodu.HizmetGuncellendi:
                default:
                    TempData["Basarili"] = "Hizmet başarıyla güncellendi";
                    return RedirectToAction("OnHizmetDuzenle", new { onhizmetId = onhizmetEkle_REQ.Id });
            }

            return View(new OnHizmetEkle_RES()
            {
                OnHizmetEkle_REQ = onhizmetEkle_REQ,
            });
        }
    }
}
