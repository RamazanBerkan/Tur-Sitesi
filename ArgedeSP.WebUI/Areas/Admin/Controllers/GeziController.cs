using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Helpers.Extantions;
using ArgedeSP.Contracts.Interfaces.BusinessLogicLayers;
using ArgedeSP.Contracts.Models.Common;
using ArgedeSP.Contracts.Models.DTO.Urunler.Req;
using ArgedeSP.Contracts.Models.DTO.Urunler.Res;
using ArgedeSP.Contracts.Models.DTO.UrunlerKategori.Req;
using ArgedeSP.Contracts.Models.DTO.UrunlerKategori.Res;
using ArgedeSP.WebUI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class GeziController : Controller
    {
        private IUrunKategoriBS _urunKategoriBS;
        private IUrunBS _urunBS;
        private readonly IRazorPartialToStringRenderer _renderer;
        public GeziController(
            IUrunKategoriBS urunKategoriBS,
            IUrunBS urunBS,
            IRazorPartialToStringRenderer renderer)
        {
            _urunKategoriBS = urunKategoriBS;
            _urunBS = urunBS;
            _renderer = renderer;
        }

        [HttpGet]
        public IActionResult Gezi()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GeziGetir()
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
            string urunAdi = dict["columns[2][search][value]"];
            string urunKategori = dict["columns[4][search][value]"];
            int durum_int = 0;
            int.TryParse(dict["columns[5][search][value]"], out durum_int);


            OperationResult veriListeleme_OR = _urunBS.UrunleriSayfala(sayfa2, sayfaBoyutu, id, urunAdi, urunKategori, (Durum)durum_int);
            if (veriListeleme_OR.IsSuccess)
            {
                VeriListeleme veriListeleme = (VeriListeleme)veriListeleme_OR.ReturnObject;

                return Json(new
                {
                    iTotalRecords = veriListeleme.ToplamVeri,
                    iTotalDisplayRecords = veriListeleme.ToplamVeri,
                    sEcho = 0,
                    sColumns = "",
                    aaData = ((List<Urun>)veriListeleme.Veri).Select(x => new
                    {
                        x.Id,
                        UrunResmi = x.Resim,
                        x.UrunAdi,
                        x.Durum,
                        x.KisaAciklama,
                        x.Keywords,
                        OlusturmaTarihi = x.OlusturmaTarihi.ToString("f"),
                        KategoriAdi = x.UrunKategori.Ad,
                        EditUrun = new
                        {
                            x.Id,
                            SeoUrl = x.UrunAdi.FriendlyUrl()
                        },
                        //x.Fiyat,
                        x.Dil
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
        [HttpPost]
        public async Task<JsonResult> GeziSil(int urunId)
        {
            if (urunId == 0)
            {
                return Json(new Genel_RES()
                {
                    BasariliMi = false,
                    Mesaj = "Gezi bulunamadı",
                });
            }

            OperationResult urunSil_OR = await _urunBS.UrunSil(urunId);

            if (urunSil_OR.IsSuccess)
            {
                return Json(new Genel_RES()
                {
                    BasariliMi = true,
                    Mesaj = "Gezi başarıyla silindi."
                });
            }

            Genel_RES sonuc = new Genel_RES();
            sonuc.BasariliMi = false;

            switch (urunSil_OR.Message)
            {
                case MesajKodu.UrunBulunamadi:
                    sonuc.Mesaj = "Gezi bulunamadı";
                    break;
                case MesajKodu.UrunSipariseEkli:
                    sonuc.Mesaj = "Gezi siparişleri var, Silinemez";
                    break;
                default:
                    sonuc.Mesaj = "Beklenmedik bir hata meydana geldi";
                    break;
            }

            return Json(sonuc);
        }
        [HttpGet]
        public async Task<IActionResult> GeziEkle()
        {
            UrunEkle_RES urunEkle_RES = new UrunEkle_RES()
            {
                UrunEkle_REQ = new UrunEkle_REQ(),
                UrunKategorileri = (List<UrunKategori>)(await _urunKategoriBS.UrunKategorileriGetir()).ReturnObject
            };

            return View(urunEkle_RES);
        }
        [HttpPost]
        public async Task<IActionResult> GeziEkle([FromForm] UrunEkle_REQ urunEkle_REQ)
        {
            UrunEkle_RES urunEkle_RES = new UrunEkle_RES()
            {
                UrunEkle_REQ = urunEkle_REQ,
                UrunKategorileri = (List<UrunKategori>)(await _urunKategoriBS.UrunKategorileriGetir()).ReturnObject
            };

            if (!ModelState.IsValid)
            {
                return View(urunEkle_RES);
            }

            OperationResult urunEkle_OR = await _urunBS.UrunEkle(urunEkle_REQ);
            if (!urunEkle_OR.IsSuccess)
            {
                switch (urunEkle_OR.Message)
                {
                    case MesajKodu.UrunKategoriBulunamadı:
                        ModelState.AddModelError("UrunEkle_REQ.UrunKategoriId", "Kategori bulunamadı");
                        break;
                    case MesajKodu.SeoUrlZatenVar:
                        ModelState.AddModelError("UrunEkle_REQ.SeoUrl", "Seo url zaten kayıtlı");
                        break;
                    case MesajKodu.BeklenmedikHata:
                    default:
                        ModelState.AddModelError("UrunEkle_REQ.BeklenmedikHata", "Kategori bulunamadı");
                        break;
                }

                return View(urunEkle_RES);
            }


            TempData["Basarili"] = "Gezi Başarıyla Eklendi";
            return RedirectToAction("GeziDuzenle", new { urunId = ((Urun)urunEkle_OR.ReturnObject).Id });
        }
        [HttpGet]
        public async Task<IActionResult> GeziDuzenle(int urunId)
        {
            OperationResult urun_OR = await _urunBS.UrunGetirSorgusuz(urunId);
            if (!urun_OR.IsSuccess || urun_OR.ReturnObject == null)
            {
                TempData["Hata"] = "Düzenlemek istediğiniz çözüm bulunamadı";
                return RedirectToAction("Gezi");
            }
            Urun urun = (Urun)urun_OR.ReturnObject;

            UrunEkle_RES urunEkle_RES = new UrunEkle_RES()
            {
                UrunEkle_REQ = new UrunEkle_REQ()
                {
                    AnaResim = urun.Resim,
                    Durum = urun.Durum == Durum.Aktif ? true : false,
                    KisaAciklama = urun.KisaAciklama,
                    //UrunModel = urun.UrunModel,
                    ResimGalerisi = urun.UrunResimGalerisi.Select(x => x.ResimUrl).ToList(),
                    Dokumanlar = urun.UrunDokumanlari.Select(x => x.DokumanUrl).ToList(),
                    UrunAdi = urun.UrunAdi,
                    UrunKategoriId = urun.UrunKategoriId,
                    UzunAciklama = urun.UzunAciklama,
                    DetayAciklama = urun.DetayAciklama,
                    UrunSira = urun.UrunSira,
                    TeknikOzellikler = urun.TeknikOzellikler,
                    Keywords = urun.Keywords,
                    NasilCalisir = urun.NasilCalisir,
                    Id = urun.Id,
                    SeoUrl = urun.SeoUrl,
                    Fiyat = urun.Fiyat,
                    Indirim = urun.Indirim,
                    IndirimliFiyat = urun.IndirimliFiyat,
                    Dil = urun.Dil
                },

                UrunKategorileri = (List<UrunKategori>)(_urunKategoriBS.UrunKategorileriGetir(Dil.Turkce, int.MaxValue)).ReturnObject,
                Urun = urun
            };
            urunEkle_RES.UrunEkle_REQ.UrunDokumanlari = urun.UrunDokumanlari.ToList();

            return View(urunEkle_RES);
        }
        [HttpPost]
        public async Task<IActionResult> GeziDuzenle([FromForm] UrunEkle_REQ urunEkle_REQ)
        {
            OperationResult urun_OR = await _urunBS.UrunGetirSorgusuz(urunEkle_REQ.Id);
            if (!urun_OR.IsSuccess || urun_OR.ReturnObject == null)
            {
                TempData["Hata"] = "Düzenlemek istediğiniz Gezi bulunamadı";
                return RedirectToAction("Gezi");
            }
            Urun urun = (Urun)urun_OR.ReturnObject;
            UrunEkle_RES urunEkle_RES = new UrunEkle_RES()
            {
                UrunEkle_REQ = urunEkle_REQ,
                UrunKategorileri = (List<UrunKategori>)(await _urunKategoriBS.UrunKategorileriGetir()).ReturnObject,
                Urun = urun
            };

            if (!ModelState.IsValid)
            {
                return View(urunEkle_RES);
            }


            OperationResult urunGuncelle_OR = await _urunBS.UrunGuncelle(urunEkle_REQ);
            if (!urunGuncelle_OR.IsSuccess)
            {
                switch (urunGuncelle_OR.Message)
                {
                    case MesajKodu.UrunBulunamadi:
                        TempData["Hata"] = "Gezi bulunamadı";
                        return RedirectToAction("Gezi", "Gezi");
                    case MesajKodu.UrunKategoriBulunamadı:
                        ModelState.AddModelError("UrunEkle_REQ.UrunKategoriId", "Kategori bulunamadı");
                        break;
                    case MesajKodu.SeoUrlZatenVar:
                        ModelState.AddModelError("UrunEkle_REQ.SeoUrl", "Seo url zaten kayıtlı");
                        break;
                    case MesajKodu.BeklenmedikHata:
                    default:
                        ModelState.AddModelError("UrunEkle_REQ.BeklenmedikHata", "Seo url zaten kayıtlı");
                        break;
                }

                return View(urunEkle_RES);
            }

            TempData["Basarili"] = "Gezi başarıyla güncellendi";
            return RedirectToAction("GeziDuzenle", new { urunId = urunEkle_REQ.Id });
        }
        [HttpGet]
        public IActionResult GeziKategorileri()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GeziKategorileriGetir()
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
            string seoUrl = dict["columns[5][search][value]"];


            OperationResult veriListeleme_OR = _urunKategoriBS.UrunKategorileriniSayfala(sayfa2, sayfaBoyutu, id, ad, seoUrl);

            if (veriListeleme_OR.IsSuccess)
            {
                VeriListeleme veriListeleme = (VeriListeleme)veriListeleme_OR.ReturnObject;

                return Json(new
                {
                    iTotalRecords = veriListeleme.ToplamVeri,
                    iTotalDisplayRecords = veriListeleme.ToplamVeri,
                    sEcho = 0,
                    sColumns = "",
                    aaData = ((List<UrunKategori>)veriListeleme.Veri).Select(x => new
                    {
                        x.Id,
                        x.Ad,
                        x.ArkaPlanResim,
                        x.KisaAciklama,
                        OlusturmaTarihi = x.OlusturmaTarihi.ToString("f"),
                        x.Resim,
                        x.SeoUrl,
                        x.Dil
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
        public IActionResult GeziKategorisiEkle()
        {
            return View(new UrunKategoriEkle_RES()
            {
                UrunKategoriEkle_REQ = new UrunKategoriEkle_REQ(),
            });
        }
        [HttpPost]
        public async Task<IActionResult> GeziKategorisiEkle([FromForm] UrunKategoriEkle_REQ urunKategoriEkle_REQ)
        {
            if (!ModelState.IsValid)
            {
                return View(new UrunKategoriEkle_RES()
                {
                    UrunKategoriEkle_REQ = urunKategoriEkle_REQ,
                });
            }

            if (urunKategoriEkle_REQ.UstId == 0)
            {
                urunKategoriEkle_REQ.UstId = null;
            }


            OperationResult urunKategoriEkle_OR = await _urunKategoriBS.UrunKategorisiEkle(urunKategoriEkle_REQ);
            if (urunKategoriEkle_OR.IsSuccess)
            {
                TempData["Basarili"] = "Kategori başarıyla eklendi";
                return RedirectToAction("GeziKategorisiEkle");
            }

            switch (urunKategoriEkle_OR.Message)
            {
                case MesajKodu.SeoUrlZatenVar:
                    ModelState.AddModelError("UrunKategoriEkle_REQ.SeoUrl", "Bu seo url zaten kayıtlı, Lütfen değiştiriniz");
                    break;
                default:
                    TempData["Basarili"] = "Kategori başarıyla eklendi";
                    return RedirectToAction("GeziKategorisiEkle");
            }

            return View(new UrunKategoriEkle_RES()
            {
                UrunKategoriEkle_REQ = urunKategoriEkle_REQ,
            });


        }
        [HttpPost]
        public async Task<JsonResult> GeziKategoriSil(int kategoriId)
        {
            if (kategoriId == 0)
            {
                return Json(new Genel_RES()
                {
                    BasariliMi = false,
                    Mesaj = "Kategori bulunamadı",
                });
            }

            OperationResult urunkategoriSil_OR = await _urunKategoriBS.UrunKategoriSil(kategoriId);

            if (urunkategoriSil_OR.IsSuccess)
            {
                return Json(new Genel_RES()
                {
                    BasariliMi = true,
                    Mesaj = "Kategori başarıyla silindi."
                });
            }

            Genel_RES sonuc = new Genel_RES();
            sonuc.BasariliMi = false;

            switch (urunkategoriSil_OR.Message)
            {
                case MesajKodu.ProjeKategoriBulunamadi:
                    sonuc.Mesaj = "Kategori bulunamadi";
                    break;
                default:
                    sonuc.Mesaj = "Beklenmedik bir hata meydana geldi";
                    break;
            }

            return Json(sonuc);
        }
        [HttpGet]
        public async Task<IActionResult> GeziKategoriDuzenle(int kategoriId)
        {
            if (kategoriId == 0)
            {
                TempData["Hata"] = "Kategori bulunamadi";
                return RedirectToAction("GeziKategorileri");
            }

            OperationResult urunkategori_OR = await _urunKategoriBS.UrunKategorisiGetirIdIle(kategoriId);
            UrunKategori urunKategori = (UrunKategori)urunkategori_OR.ReturnObject;
            if (!urunkategori_OR.IsSuccess || urunKategori == null)
            {
                TempData["Hata"] = "Kategori bulunamadi";
                return RedirectToAction("GeziKategorileri");
            }

            return View(new UrunKategoriEkle_RES()
            {
                UrunKategoriEkle_REQ = new UrunKategoriEkle_REQ()
                {

                    Ad = urunKategori.Ad,
                    ArkaPlanResim = urunKategori.ArkaPlanResim,
                    Id = urunKategori.Id,
                    KisaAciklama = urunKategori.KisaAciklama,
                    UstId = (urunKategori.UstId ?? null),
                    Resim = urunKategori.Resim,
                    NavbarResim = urunKategori.NavbarResim,
                    SeoUrl = urunKategori.SeoUrl,
                    UzunAciklama = urunKategori.UzunAciklama,
                    Dil = urunKategori.Dil,
                    AnaDilcesi = urunKategori.AnaDilcesi
                }
            });

        }
        [HttpPost]
        public async Task<IActionResult> GeziKategoriDuzenle([FromForm] UrunKategoriEkle_REQ urunKategoriEkle_REQ)
        {
            if (!ModelState.IsValid)
            {
                return View(new UrunKategoriEkle_RES()
                {
                    UrunKategoriEkle_REQ = urunKategoriEkle_REQ,
                });
            }

            if (urunKategoriEkle_REQ.Id == 0)
            {
                TempData["Hata"] = "Kategori Bulunamadı";
                return RedirectToAction("GeziKategorileri");
            }

            if (urunKategoriEkle_REQ.UstId == 0) { urunKategoriEkle_REQ.UstId = null; }


            OperationResult urunKategoriEkle_OR = await _urunKategoriBS.UrunKategoriGuncelle(urunKategoriEkle_REQ);
            if (urunKategoriEkle_OR.IsSuccess)
            {
                TempData["Basarili"] = "Kategori başarıyla güncellendi";
                return RedirectToAction("GeziKategoriDuzenle", new { kategoriId = urunKategoriEkle_REQ.Id });
            }

            switch (urunKategoriEkle_OR.Message)
            {
                case MesajKodu.SeoUrlZatenVar:
                    ModelState.AddModelError("ProjeKategoriEkle_REQ.SeoUrl", "Bu seo url zaten kayıtlı, Lütfen değiştiriniz");
                    break;
                case MesajKodu.ProjeKategoriGuncellendi:
                default:
                    TempData["Basarili"] = "Kategori başarıyla güncellendi";
                    return RedirectToAction("KategoriDuzenle", new { kategoriId = urunKategoriEkle_REQ.Id });
            }

            return View(new UrunKategoriEkle_RES()
            {
                UrunKategoriEkle_REQ = urunKategoriEkle_REQ,
            });
        }

        [HttpPost]
        public JsonResult GeziDilKategorileri(int dil)
        {
            OperationResult kategoriDil_OR = _urunKategoriBS.UrunKategorileriGetir((Dil)dil, int.MaxValue);
            List<UrunKategori> projeKategorileri = (List<UrunKategori>)kategoriDil_OR.ReturnObject;

            return Json(new Genel_RES()
            {
                BasariliMi = true,
                Model = projeKategorileri.Select(x => new
                {
                    KategoriId = x.Id,
                    KategoriAdi = x.Ad + " -- (" + x.AnaDilcesi + ")"
                }).ToList()
            });
        }
    }
}
