using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Helpers;
using ArgedeSP.Contracts.Interfaces.BusinessLogicLayers;
using ArgedeSP.Contracts.Models.Common;
using ArgedeSP.Contracts.Models.DTO.Ayarlar.Req;
using ArgedeSP.Contracts.Models.DTO.Hakkimizda.Req;
using ArgedeSP.Contracts.Models.DTO.Hakkimizda.Res;
using ArgedeSP.Contracts.Models.DTO.Kurumsal.Req;
using ArgedeSP.Contracts.Models.DTO.Kurumsal.Res;
using ArgedeSP.Contracts.Models.DTO.Hizmet.Req;
using ArgedeSP.Contracts.Models.DTO.Hizmet.Res;
using ArgedeSP.Contracts.Models.DTO.OnlineSatis.Req;
using ArgedeSP.Contracts.Models.DTO.OnlineSatis.Res;
using ArgedeSP.Contracts.Models.DTO.Sorular.Req;
using ArgedeSP.Contracts.Models.DTO.Sorular.Res;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AnahtarDegerController : Controller
    {
        private IAnahtarDegerBS _anahtarDegerBS;
        private ISliderResimBS _sliderResimBS;
        private IUrunKategoriBS _urunKategoriBS;

        public AnahtarDegerController(
            IAnahtarDegerBS anahtarDegerBS,
            ISliderResimBS sliderResimBS,
            IUrunKategoriBS urunKategoriBS)
        {
            _sliderResimBS = sliderResimBS;
            _anahtarDegerBS = anahtarDegerBS;
            _urunKategoriBS = urunKategoriBS;
        }

        #region Hakkımızda
        public IActionResult HakkimizdaSayfasi()
        {
            OperationResult hakkimizda_OR = _anahtarDegerBS.AnahtarGetir(Tanimlamalar.HakkimizdaSayfasi);

            List<AnahtarDeger> hakkimizdalar = (List<AnahtarDeger>)hakkimizda_OR.ReturnObject ?? new List<AnahtarDeger>();


            hakkimizda_OR = _anahtarDegerBS.AnahtarGetir(Tanimlamalar.Hakkımızda1Sayfasi);
            hakkimizdalar.AddRange((List<AnahtarDeger>)hakkimizda_OR.ReturnObject ?? new List<AnahtarDeger>());

            hakkimizda_OR = _anahtarDegerBS.AnahtarGetir(Tanimlamalar.Hakkımızda2Sayfasi);
            hakkimizdalar.AddRange((List<AnahtarDeger>)hakkimizda_OR.ReturnObject ?? new List<AnahtarDeger>());

            hakkimizda_OR = _anahtarDegerBS.AnahtarGetir(Tanimlamalar.HedefSayfasi);
            hakkimizdalar.AddRange((List<AnahtarDeger>)hakkimizda_OR.ReturnObject ?? new List<AnahtarDeger>());

            hakkimizda_OR = _anahtarDegerBS.AnahtarGetir(Tanimlamalar.VizyonMisyonSayfasi);
            hakkimizdalar.AddRange((List<AnahtarDeger>)hakkimizda_OR.ReturnObject ?? new List<AnahtarDeger>());

            



            return View(hakkimizdalar);
        }
        [HttpGet]
        public IActionResult HakkimizdaSayfasiDuzenle(int anahtarDegerId)
        {
            OperationResult hakkimizda_OR = _anahtarDegerBS.AnahtarGetir(anahtarDegerId);
            AnahtarDeger hakkimizda = (AnahtarDeger)hakkimizda_OR.ReturnObject;

            if (hakkimizda == null)
            {
                TempData["Hata"] = "Hakkımızda Sayfası bulunamadı";
                return RedirectToAction("HakkimizdaSayfasi");
            }

            return View(new Hakkimizda_RES()
            {
                Dil = hakkimizda.Dil.DilYaziyaCevir(),
                Hakkimizda_REQ = new Hakkimizda_REQ()
                {
                    AnahtarId = hakkimizda.Id,
                    HakkimizdaIcerik = hakkimizda.Deger
                }
            });
        }
        [HttpPost]
        public IActionResult HakkimizdaSayfasiDuzenle([FromForm] Hakkimizda_REQ hakkimizda_REQ)
        {
            OperationResult hakkimizda_OR = _anahtarDegerBS.AnahtarGetir(hakkimizda_REQ.AnahtarId);
            AnahtarDeger hakkimizda = (AnahtarDeger)hakkimizda_OR.ReturnObject;

            if (hakkimizda == null)
            {
                TempData["Hata"] = "Hakkımızda Sayfası bulunamadı";
                return RedirectToAction("HakkimizdaSayfasi");
            }

            if (!ModelState.IsValid)
            {
                return View(new Hakkimizda_RES()
                {
                    Dil = hakkimizda.Dil.DilYaziyaCevir(),
                    Hakkimizda_REQ = hakkimizda_REQ
                });
            }

            hakkimizda.Deger = hakkimizda_REQ.HakkimizdaIcerik;

            _anahtarDegerBS.AnahtarGuncelle(hakkimizda);

            TempData["Basarili"] = "Güncelleme işlemi başarılı";
            return RedirectToAction("HakkimizdaSayfasiDuzenle", new { anahtarDegerId = hakkimizda_REQ.AnahtarId });
        }
        #endregion

        #region Kurumsal
        //public IActionResult KurumsalSayfasi()
        //{
        //    OperationResult kurumsal_OR = _anahtarDegerBS.AnahtarGetir(Tanimlamalar.KurumsalSayfasi);

        //    List<AnahtarDeger> kurumsallar = (List<AnahtarDeger>)kurumsal_OR.ReturnObject ?? new List<AnahtarDeger>();

        //    return View(kurumsallar);
        //}
        [HttpGet]
        public IActionResult KurumsalSayfasiDuzenle(int anahtarDegerId)
        {
            OperationResult kurumsal_OR = _anahtarDegerBS.AnahtarGetir(anahtarDegerId);
            AnahtarDeger kurumsal = (AnahtarDeger)kurumsal_OR.ReturnObject;

            if (kurumsal == null)
            {
                TempData["Hata"] = "Kurumsal Sayfası bulunamadı";
                return RedirectToAction("KurumsalSayfasi");
            }

            return View(new Kurumsal_RES()
            {
                Dil = kurumsal.Dil.DilYaziyaCevir(),
                Kurumsal_REQ = new Kurumsal_REQ()
                {
                    AnahtarId = kurumsal.Id,
                    KurumsalIcerik = kurumsal.Deger
                }
            });
        }
        [HttpPost]
        public IActionResult KurumsalSayfasiDuzenle([FromForm] Kurumsal_REQ kurumsal_REQ)
        {
            OperationResult kurumsal_OR = _anahtarDegerBS.AnahtarGetir(kurumsal_REQ.AnahtarId);
            AnahtarDeger kurumsal = (AnahtarDeger)kurumsal_OR.ReturnObject;

            if (kurumsal == null)
            {
                TempData["Hata"] = "Kurumsal Sayfası bulunamadı";
                return RedirectToAction("KurumsalSayfasi");
            }

            if (!ModelState.IsValid)
            {
                return View(new Kurumsal_RES()
                {
                    Dil = kurumsal.Dil.DilYaziyaCevir(),
                    Kurumsal_REQ = kurumsal_REQ
                });
            }

            kurumsal.Deger = kurumsal_REQ.KurumsalIcerik;

            _anahtarDegerBS.AnahtarGuncelle(kurumsal);

            TempData["Basarili"] = "Güncelleme işlemi başarılı";
            return RedirectToAction("KurumsalSayfasiDuzenle", new { anahtarDegerId = kurumsal_REQ.AnahtarId });
        }
        #endregion

        #region Hizmet
        //public IActionResult HizmetSayfasi()
        //{
        //    OperationResult hizmet_OR = _anahtarDegerBS.AnahtarGetir(Tanimlamalar.HizmetSayfasi);

        //    List<AnahtarDeger> hizmetlers = (List<AnahtarDeger>)hizmet_OR.ReturnObject ?? new List<AnahtarDeger>();

        //    return View(hizmetlers);
        //}
        [HttpGet]
        public IActionResult HizmetSayfasiDuzenle(int anahtarDegerId)
        {
            OperationResult hizmet_OR = _anahtarDegerBS.AnahtarGetir(anahtarDegerId);
            AnahtarDeger hizmet = (AnahtarDeger)hizmet_OR.ReturnObject;

            if (hizmet == null)
            {
                TempData["Hata"] = "Hizmet Sayfası bulunamadı";
                return RedirectToAction("HizmetSayfasi");
            }

            return View(new Hizmet_RES()
            {
                Dil = hizmet.Dil.DilYaziyaCevir(),
                Hizmet_REQ = new Hizmet_REQ()
                {
                    AnahtarId = hizmet.Id,
                    HizmetIcerik = hizmet.Deger
                }
            });
        }
        [HttpPost]
        public IActionResult HizmetlSayfasiDuzenle([FromForm] Hizmet_REQ hizmet_REQ)
        {
            OperationResult hizmet_OR = _anahtarDegerBS.AnahtarGetir(hizmet_REQ.AnahtarId);
            AnahtarDeger hizmet = (AnahtarDeger)hizmet_OR.ReturnObject;

            if (hizmet == null)
            {
                TempData["Hata"] = "Hizmet Sayfası bulunamadı";
                return RedirectToAction("HizmetSayfasi");
            }

            if (!ModelState.IsValid)
            {
                return View(new Hizmet_RES()
                {
                    Dil = hizmet.Dil.DilYaziyaCevir(),
                    Hizmet_REQ = hizmet_REQ
                });
            }

            hizmet.Deger = hizmet_REQ.HizmetIcerik;

            _anahtarDegerBS.AnahtarGuncelle(hizmet);

            TempData["Basarili"] = "Güncelleme işlemi başarılı";
            return RedirectToAction("HizmetSayfasiDuzenle", new { anahtarDegerId = hizmet_REQ.AnahtarId });
        }
        #endregion

        #region Sorular
        public IActionResult SorularSayfasi()
        {
            OperationResult sorular_OR = _anahtarDegerBS.AnahtarGetir(Tanimlamalar.SorularSayfasi);

            List<AnahtarDeger> sorularlar = (List<AnahtarDeger>)sorular_OR.ReturnObject ?? new List<AnahtarDeger>();

            return View(sorularlar);
        }
        [HttpGet]
        public IActionResult SorularSayfasiDuzenle(int anahtarDegerId)
        {
            OperationResult sorular_OR = _anahtarDegerBS.AnahtarGetir(anahtarDegerId);
            AnahtarDeger sorular = (AnahtarDeger)sorular_OR.ReturnObject;

            if (sorular == null)
            {
                TempData["Hata"] = "Sorular Sayfası bulunamadı";
                return RedirectToAction("SorularSayfasi");
            }

            return View(new Sorular_RES()
            {
                Dil = sorular.Dil.DilYaziyaCevir(),
                Sorular_REQ = new Sorular_REQ()
                {
                    AnahtarId = sorular.Id,
                    SorularIcerik = sorular.Deger
                }
            });
        }
        [HttpPost]
        public IActionResult SorularSayfasiDuzenle([FromForm]Sorular_REQ sorular_REQ)
        {
            OperationResult sorular_OR = _anahtarDegerBS.AnahtarGetir(sorular_REQ.AnahtarId);
            AnahtarDeger sorular = (AnahtarDeger)sorular_OR.ReturnObject;

            if (sorular == null)
            {
                TempData["Hata"] = "Sorular Sayfası bulunamadı";
                return RedirectToAction("SorularSayfasi");
            }

            if (!ModelState.IsValid)
            {
                return View(new Sorular_RES()
                {
                    Dil = sorular.Dil.DilYaziyaCevir(),
                    Sorular_REQ = sorular_REQ
                });
            }

            sorular.Deger = sorular_REQ.SorularIcerik;

            _anahtarDegerBS.AnahtarGuncelle(sorular);

            TempData["Basarili"] = "Güncelleme işlemi başarılı";
            return RedirectToAction("SorularSayfasiDuzenle", new { anahtarDegerId = sorular_REQ.AnahtarId });
        }
        #endregion

        #region OnlineSatis
        public IActionResult OnlineSatisSayfasi()
        {
            OperationResult onlinesatis_OR = _anahtarDegerBS.AnahtarGetir(Tanimlamalar.OnlineSatisSayfasi);

            List<AnahtarDeger> OnlineSatislar = (List<AnahtarDeger>)onlinesatis_OR.ReturnObject ?? new List<AnahtarDeger>();

            return View(OnlineSatislar);
        }
        [HttpGet]
        //public IActionResult GizlilikPolitikasiSayfasiDuzenle(int anahtarDegerId)
        //{
        //    OperationResult gizlilikPolitikasi_OR = _anahtarDegerBS.AnahtarGetir(anahtarDegerId);
        //    AnahtarDeger gizlilikPolitikasi = (AnahtarDeger)gizlilikPolitikasi_OR.ReturnObject;

        //    if (gizlilikPolitikasi == null)
        //    {
        //        TempData["Hata"] = "Gizlilik Politikası Sayfası bulunamadı";
        //        return RedirectToAction("GizlilikPolitikasiSayfasi");
        //    }

        //    return View(new GizlilikPolitikasi_RES()
        //    {
        //        Dil = gizlilikPolitikasi.Dil.DilYaziyaCevir(),
        //        GizlilikPolitikasi_REQ = new GizlilikPolitikasi_REQ()
        //        {
        //            AnahtarId = gizlilikPolitikasi.Id,
        //            GizlilikPolitikasiIcerik = gizlilikPolitikasi.Deger
        //        }
        //    });
        //}
        //[HttpPost]
        //public IActionResult GizlilikPolitikasiSayfasiDuzenle([FromForm]GizlilikPolitikasi_REQ gizlilikPolitikasi_REQ)
        //{
        //    OperationResult gizlilikPolitikasi_OR = _anahtarDegerBS.AnahtarGetir(gizlilikPolitikasi_REQ.AnahtarId);
        //    AnahtarDeger gizlilikPolitikasi = (AnahtarDeger)gizlilikPolitikasi_OR.ReturnObject;

        //    if (gizlilikPolitikasi == null)
        //    {
        //        TempData["Hata"] = "Gizlilik Politikası Sayfası bulunamadı";
        //        return RedirectToAction("GizlilikPolitikasiSayfasi");
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        return View(new GizlilikPolitikasi_RES()
        //        {
        //            Dil = gizlilikPolitikasi.Dil.DilYaziyaCevir(),
        //            GizlilikPolitikasi_REQ = gizlilikPolitikasi_REQ
        //        });
        //    }

        //    gizlilikPolitikasi.Deger = gizlilikPolitikasi_REQ.GizlilikPolitikasiIcerik;

        //    _anahtarDegerBS.AnahtarGuncelle(gizlilikPolitikasi);

        //    TempData["Basarili"] = "Güncelleme işlemi başarılı";
        //    return RedirectToAction("GizlilikPolitikasiSayfasiDuzenle", new { anahtarDegerId = gizlilikPolitikasi_REQ.AnahtarId });
        //}
        #endregion
        #region Foto-Galeri
        public IActionResult FotoGaleriSayfasi()
        {
            OperationResult resimGalerisi_OR = _anahtarDegerBS.AnahtarGetir(Tanimlamalar.ResimGalerisi);

            AnahtarDeger resimGaleri = ((List<AnahtarDeger>)resimGalerisi_OR.ReturnObject).FirstOrDefault();
            List<ResimGalerisi> resimGalerisi = null;

            if (resimGaleri != null)
            {
                resimGalerisi = JsonConvert.DeserializeObject<List<ResimGalerisi>>(resimGaleri.Deger);
            }

            ViewBag.FotoGalerileri = resimGalerisi.OrderBy(x => x.Sira).ToList() ?? new List<ResimGalerisi>();

            OperationResult kategoriDil_OR = _urunKategoriBS.UrunKategorileriGetir(Dil.Turkce, int.MaxValue);
            List<UrunKategori> projeKategorileri = (List<UrunKategori>)kategoriDil_OR.ReturnObject;
            ViewBag.UrunKategorileri = projeKategorileri;

            return View(new ResimGalerisi());
        }
        [HttpPost]
        public async Task<IActionResult> FotoGaleriSayfasi([FromForm]ResimGalerisi resimGalerisi)
        {
            OperationResult resimGalerisi_OR = _anahtarDegerBS.AnahtarGetir(Tanimlamalar.ResimGalerisi);
            AnahtarDeger resimGaleri = ((List<AnahtarDeger>)resimGalerisi_OR.ReturnObject).FirstOrDefault();
            List<ResimGalerisi> resimGalerisiModel = null;

            if (resimGaleri != null)
            {
                resimGalerisiModel = JsonConvert.DeserializeObject<List<ResimGalerisi>>(resimGaleri.Deger);
            }

            ViewBag.FotoGalerileri = resimGalerisiModel.OrderBy(x => x.Sira).ToList() ?? new List<ResimGalerisi>();

            OperationResult kategoriDil_OR = _urunKategoriBS.UrunKategorileriGetir(Dil.Turkce, int.MaxValue);
            List<UrunKategori> projeKategorileri = (List<UrunKategori>)kategoriDil_OR.ReturnObject;
            ViewBag.UrunKategorileri = projeKategorileri;

            if (!ModelState.IsValid)
            {
                return View(resimGalerisi);
            }

            UrunKategori urunKategori = (UrunKategori)(await _urunKategoriBS.UrunKategorisiGetirIdIle(resimGalerisi.UrunKategoriId)).ReturnObject;

            resimGalerisiModel.Add(new ResimGalerisi()
            {
                Id = Guid.NewGuid().ToString(),
                ResimYolu = resimGalerisi.ResimYolu,
                Sira = resimGalerisi.Sira,
                KategoriAdi = urunKategori.Ad,
                UrunKategoriId = urunKategori.Id
            });

            resimGaleri.Deger = JsonConvert.SerializeObject(resimGalerisiModel);

            _anahtarDegerBS.AnahtarGuncelle(resimGaleri);

            return RedirectToAction("FotoGaleriSayfasi");
        }
        [HttpPost]
        public JsonResult FotoGaleriSil(string resimId)
        {
            OperationResult resimGalerisi_OR = _anahtarDegerBS.AnahtarGetir(Tanimlamalar.ResimGalerisi);
            AnahtarDeger resimGaleri = ((List<AnahtarDeger>)resimGalerisi_OR.ReturnObject).FirstOrDefault();
            List<ResimGalerisi> resimGalerisi = null;

            if (resimGaleri != null)
            {
                resimGalerisi = JsonConvert.DeserializeObject<List<ResimGalerisi>>(resimGaleri.Deger);
            }
            ResimGalerisi silinecek = resimGalerisi.FirstOrDefault(x => x.Id == resimId);
            if (silinecek == null)
            {
                return Json(new Genel_RES()
                {
                    BasariliMi = false,
                    Mesaj = "Resim bulunamadi"
                });
            }

            resimGalerisi.Remove(silinecek);
            resimGaleri.Deger = JsonConvert.SerializeObject(resimGalerisi);

            _anahtarDegerBS.AnahtarGuncelle(resimGaleri);

            return Json(new Genel_RES()
            {
                BasariliMi = true,
                Mesaj = "Resim başarıyla silindi"
            });

        }
        [HttpPost]
        public JsonResult FotoGaleriGuncelle(string resimId, int siraNo)
        {
            OperationResult resimGalerisi_OR = _anahtarDegerBS.AnahtarGetir(Tanimlamalar.ResimGalerisi);
            AnahtarDeger resimGaleri = ((List<AnahtarDeger>)resimGalerisi_OR.ReturnObject).FirstOrDefault();
            List<ResimGalerisi> resimGalerisi = null;

            if (resimGaleri != null)
            {
                resimGalerisi = JsonConvert.DeserializeObject<List<ResimGalerisi>>(resimGaleri.Deger);
            }

            ResimGalerisi degistirilecek = resimGalerisi.FirstOrDefault(x => x.Id == resimId);
            if (degistirilecek == null)
            {
                return Json(new Genel_RES()
                {
                    BasariliMi = false,
                    Mesaj = "Resim bulunamadi"
                });
            }

            degistirilecek.Sira = siraNo;
            resimGaleri.Deger = JsonConvert.SerializeObject(resimGalerisi);

            _anahtarDegerBS.AnahtarGuncelle(resimGaleri);

            return Json(new Genel_RES()
            {
                BasariliMi = true,
            });

        }
        [HttpPost]
        public async Task<JsonResult> FotoGaleriGuncelle2(string resimId, int kategoriId)
        {
            OperationResult resimGalerisi_OR = _anahtarDegerBS.AnahtarGetir(Tanimlamalar.ResimGalerisi);
            AnahtarDeger resimGaleri = ((List<AnahtarDeger>)resimGalerisi_OR.ReturnObject).FirstOrDefault();
            List<ResimGalerisi> resimGalerisi = null;

            if (resimGaleri != null)
            {
                resimGalerisi = JsonConvert.DeserializeObject<List<ResimGalerisi>>(resimGaleri.Deger);
            }

            ResimGalerisi degistirilecek = resimGalerisi.FirstOrDefault(x => x.Id == resimId);
            if (degistirilecek == null)
            {
                return Json(new Genel_RES()
                {
                    BasariliMi = false,
                    Mesaj = "Resim bulunamadi"
                });
            }


            UrunKategori urunKategori = (UrunKategori)(await _urunKategoriBS.UrunKategorisiGetirIdIle(kategoriId)).ReturnObject;

            degistirilecek.KategoriAdi = urunKategori.Ad;
            degistirilecek.UrunKategoriId = urunKategori.Id;

            resimGaleri.Deger = JsonConvert.SerializeObject(resimGalerisi);

            _anahtarDegerBS.AnahtarGuncelle(resimGaleri);

            return Json(new Genel_RES()
            {
                BasariliMi = true,
            });

        }
        #endregion

        #region Kullanıcı Yorumları
        public IActionResult KullaniciYorumlariSayfasi()
        {
            OperationResult kullaniciYorumlari_TR_OR = _anahtarDegerBS.AnahtarGetir(Dil.Turkce, Tanimlamalar.YorumlarSayfasi);
            AnahtarDeger kullaniciYorumlari_TR = (AnahtarDeger)kullaniciYorumlari_TR_OR.ReturnObject;
            List<Yorum> yorumlar_TR = null;

            if (kullaniciYorumlari_TR != null)
            {
                yorumlar_TR = JsonConvert.DeserializeObject<List<Yorum>>(kullaniciYorumlari_TR.Deger);
            }

            OperationResult kullaniciYorumlari_EN_OR = _anahtarDegerBS.AnahtarGetir(Dil.Ingilizce, Tanimlamalar.YorumlarSayfasi);
            AnahtarDeger kullaniciYorumlari_EN = (AnahtarDeger)kullaniciYorumlari_EN_OR.ReturnObject;
            List<Yorum> yorumlar_EN = null;

            if (kullaniciYorumlari_EN != null)
            {
                yorumlar_EN = JsonConvert.DeserializeObject<List<Yorum>>(kullaniciYorumlari_EN.Deger);
            }

            ViewBag.Yorumlar_TR = yorumlar_TR.ToList() ?? new List<Yorum>();
            ViewBag.Yorumlar_EN = yorumlar_EN.ToList() ?? new List<Yorum>();


            return View(new Yorum());
        }
        [HttpPost]
        public IActionResult KullaniciYorumlariSayfasi([FromForm]Yorum yorum, int dil)
        {
            OperationResult kullaniciYorumlari_TR_OR = _anahtarDegerBS.AnahtarGetir(Dil.Turkce, Tanimlamalar.YorumlarSayfasi);
            AnahtarDeger kullaniciYorumlari_TR = (AnahtarDeger)kullaniciYorumlari_TR_OR.ReturnObject;
            List<Yorum> yorumlar_TR = null;

            if (kullaniciYorumlari_TR != null)
            {
                yorumlar_TR = JsonConvert.DeserializeObject<List<Yorum>>(kullaniciYorumlari_TR.Deger);
            }

            OperationResult kullaniciYorumlari_EN_OR = _anahtarDegerBS.AnahtarGetir(Dil.Ingilizce, Tanimlamalar.YorumlarSayfasi);
            AnahtarDeger kullaniciYorumlari_EN = (AnahtarDeger)kullaniciYorumlari_EN_OR.ReturnObject;
            List<Yorum> yorumlar_EN = null;

            if (kullaniciYorumlari_EN != null)
            {
                yorumlar_EN = JsonConvert.DeserializeObject<List<Yorum>>(kullaniciYorumlari_EN.Deger);
            }

            if (dil == 1)
            {
                yorumlar_TR.Add(yorum);
                kullaniciYorumlari_TR.Deger = JsonConvert.SerializeObject(yorumlar_TR);
                _anahtarDegerBS.AnahtarGuncelle(kullaniciYorumlari_TR);

                return RedirectToAction("KullaniciYorumlariSayfasi");
            }
            else
            {
                yorumlar_EN.Add(yorum);
                kullaniciYorumlari_EN.Deger = JsonConvert.SerializeObject(yorumlar_EN);
                _anahtarDegerBS.AnahtarGuncelle(kullaniciYorumlari_EN);

                return RedirectToAction("KullaniciYorumlariSayfasi");
            }

        }
        [HttpPost]
        public JsonResult KullaniciYorumuSil(string yorumId, int dil)
        {
            OperationResult kullaniciYorumlari_OR = _anahtarDegerBS.AnahtarGetir(Tanimlamalar.YorumlarSayfasi);
            AnahtarDeger yorumlar = ((List<AnahtarDeger>)kullaniciYorumlari_OR.ReturnObject).FirstOrDefault(x => x.Dil == (Dil)dil);
            List<Yorum> yorumlar2 = null;

            if (yorumlar != null)
            {
                yorumlar2 = JsonConvert.DeserializeObject<List<Yorum>>(yorumlar.Deger);
            }
            Yorum silinecek = yorumlar2.FirstOrDefault(x => x.Id == yorumId);
            if (silinecek == null)
            {
                return Json(new Genel_RES()
                {
                    BasariliMi = false,
                    Mesaj = "Yorum bulunamadi"
                });
            }

            yorumlar2.Remove(silinecek);
            yorumlar.Deger = JsonConvert.SerializeObject(yorumlar2);

            _anahtarDegerBS.AnahtarGuncelle(yorumlar);

            return Json(new Genel_RES()
            {
                BasariliMi = true,
                Mesaj = "Yorum başarıyla silindi"
            });

        }
        [HttpPost]
        public JsonResult KullaniciYorumuGuncelle(string resimId, int siraNo)
        {
            OperationResult resimGalerisi_OR = _anahtarDegerBS.AnahtarGetir(Tanimlamalar.ResimGalerisi);
            AnahtarDeger resimGaleri = ((List<AnahtarDeger>)resimGalerisi_OR.ReturnObject).FirstOrDefault();
            List<ResimGalerisi> resimGalerisi = null;

            if (resimGaleri != null)
            {
                resimGalerisi = JsonConvert.DeserializeObject<List<ResimGalerisi>>(resimGaleri.Deger);
            }

            ResimGalerisi degistirilecek = resimGalerisi.FirstOrDefault(x => x.Id == resimId);
            if (degistirilecek == null)
            {
                return Json(new Genel_RES()
                {
                    BasariliMi = false,
                    Mesaj = "Resim bulunamadi"
                });
            }

            degistirilecek.Sira = siraNo;
            resimGaleri.Deger = JsonConvert.SerializeObject(resimGalerisi);

            _anahtarDegerBS.AnahtarGuncelle(resimGaleri);

            return Json(new Genel_RES()
            {
                BasariliMi = true,
            });

        }
        #endregion

        #region SiteAyarlari
        [HttpGet]
        public IActionResult SiteAyarlari()
        {
            //Proje Adi - ING-TR
            OperationResult projeAdlari_OR = _anahtarDegerBS.AnahtarGetir(Tanimlamalar.ProjeAdi);
            List<AnahtarDeger> projeAdlari = (List<AnahtarDeger>)projeAdlari_OR.ReturnObject;
            string projeAdi_TR = projeAdlari.FirstOrDefault(x => x.Dil == Dil.Turkce)?.Deger;
            string projeAdi_EN = projeAdlari.FirstOrDefault(x => x.Dil == Dil.Ingilizce)?.Deger;

            //Site Adı
            OperationResult siteAdi_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.SiteAdi);
            string siteAdi = ((AnahtarDeger)siteAdi_OR.ReturnObject).Deger;

            //Telefon
            OperationResult telefon_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.TelefonNumarasi);
            string telefon = ((AnahtarDeger)telefon_OR.ReturnObject).Deger;

            //Faks
            OperationResult faks_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Faks);
            string faks = ((AnahtarDeger)faks_OR.ReturnObject).Deger;

            //E-mail
            OperationResult email_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Email);
            string email = ((AnahtarDeger)email_OR.ReturnObject).Deger;

            //Adres - ING-TR
            OperationResult adresler_OR = _anahtarDegerBS.AnahtarGetir(Tanimlamalar.Adres);
            List<AnahtarDeger> adresler = (List<AnahtarDeger>)adresler_OR.ReturnObject;
            string adres_TR = adresler.FirstOrDefault(x => x.Dil == Dil.Turkce)?.Deger;
            string adres_EN = adresler.FirstOrDefault(x => x.Dil == Dil.Ingilizce)?.Deger;

            //Footer Yazi - ING-TR
            OperationResult footerYazilari_OR = _anahtarDegerBS.AnahtarGetir(Tanimlamalar.FooterYazi);
            List<AnahtarDeger> footerYazilari = (List<AnahtarDeger>)footerYazilari_OR.ReturnObject;
            string footerYazi_TR = footerYazilari.FirstOrDefault(x => x.Dil == Dil.Turkce)?.Deger;
            string footerYazi_EN = footerYazilari.FirstOrDefault(x => x.Dil == Dil.Ingilizce)?.Deger;

            //AnaSayfa Banner - ING-TR
            OperationResult anaSayfaBanner_OR = _anahtarDegerBS.AnahtarGetir(Tanimlamalar.AnaSayfaBanner);
            List<AnahtarDeger> anaSayfaBannerleri = (List<AnahtarDeger>)anaSayfaBanner_OR.ReturnObject;
            AnaSayfaBanner anaSayfaBanner_TR = JsonConvert.DeserializeObject<AnaSayfaBanner>(anaSayfaBannerleri.FirstOrDefault(x => x.Dil == Dil.Turkce)?.Deger);
            AnaSayfaBanner anaSayfaBanner_EN = JsonConvert.DeserializeObject<AnaSayfaBanner>(anaSayfaBannerleri.FirstOrDefault(x => x.Dil == Dil.Ingilizce)?.Deger);

            //Google Map
            OperationResult googleMap_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.GoogleMap);
            string googleMap = ((AnahtarDeger)googleMap_OR.ReturnObject).Deger;




            //Main Keywords
            OperationResult mainkeywords_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.MainKeywords);
            string mainkeywords = ((AnahtarDeger)mainkeywords_OR.ReturnObject).Deger;


            //Description
            OperationResult description_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Description);
            string description = ((AnahtarDeger)description_OR.ReturnObject).Deger;

            //iletisim açıklama 
            OperationResult iletisimAciklama_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.IletisimAciklama);
            string iletisimAciklama = ((AnahtarDeger)iletisimAciklama_OR.ReturnObject).Deger;

            //iletisim açıklama  baslıik
            OperationResult iletisimAciklamaBaslik_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.IletisimAciklamaBaslik);
            string iletisimBaslikAciklama = ((AnahtarDeger)iletisimAciklamaBaslik_OR.ReturnObject).Deger;

            //Title Sirket Adı
            OperationResult titlesirketadi_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.TitleSirketAdi);
            string titlesirketadi = ((AnahtarDeger)titlesirketadi_OR.ReturnObject).Deger;


            //Instagram
            OperationResult instagram_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Instagram);
            string instagram = ((AnahtarDeger)instagram_OR.ReturnObject).Deger;

            //Instagram
            OperationResult twitter_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Twitter);
            string twitter = ((AnahtarDeger)twitter_OR.ReturnObject).Deger;

            //facebook
            OperationResult facebook_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Facebook);
            string facebook = ((AnahtarDeger)facebook_OR.ReturnObject).Deger;

            Ayarlar_REQ ayarlar_REQ = new Ayarlar_REQ()
            {
                Adres_TR = adres_TR,
                AnaSayfaBanner_EN = anaSayfaBanner_EN,
                AnaSayfaBanner_TR = anaSayfaBanner_TR,
                Email = email,
                Facebook = facebook,
                FooterYazi_EN = footerYazi_EN,
                FooterYazi_TR = footerYazi_TR,
                GoogleMap = googleMap,
                Description = description,
                MainKeywords = mainkeywords,
                Instagram = instagram,
                ProjeAdi_TR = projeAdi_TR,
                SiteAdi = siteAdi,
                Telefon = telefon,
                Faks = faks,
                Twitter = twitter,
                IletisimAciklamaBaslik = iletisimBaslikAciklama,
                IletisimAciklama=iletisimAciklama,


            };

            return View(ayarlar_REQ);
        }
        [HttpPost]
        public IActionResult SiteAyarlari([FromForm]Ayarlar_REQ ayarlar_REQ)
        {
            if (!ModelState.IsValid)
            {
                return View(ayarlar_REQ);
            }

            //Proje Adi - ING-TR
            OperationResult projeAdlari_OR = _anahtarDegerBS.AnahtarGetir(Tanimlamalar.ProjeAdi);
            List<AnahtarDeger> projeAdlari = (List<AnahtarDeger>)projeAdlari_OR.ReturnObject;
            projeAdlari.FirstOrDefault(x => x.Dil == Dil.Turkce).Deger = ayarlar_REQ.ProjeAdi_TR;


            _anahtarDegerBS.AnahtarGuncelle(projeAdlari.FirstOrDefault(x => x.Dil == Dil.Turkce));
            _anahtarDegerBS.AnahtarGuncelle(projeAdlari.FirstOrDefault(x => x.Dil == Dil.Ingilizce));


            //Site Adı
            OperationResult siteAdi_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.SiteAdi);
            AnahtarDeger siteAdi = ((AnahtarDeger)siteAdi_OR.ReturnObject);
            siteAdi.Deger = ayarlar_REQ.SiteAdi;
            _anahtarDegerBS.AnahtarGuncelle(siteAdi);

            //Telefon
            OperationResult telefon_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.TelefonNumarasi);
            AnahtarDeger telefon = ((AnahtarDeger)telefon_OR.ReturnObject);
            telefon.Deger = ayarlar_REQ.Telefon;
            _anahtarDegerBS.AnahtarGuncelle(telefon);

            //İletisim Açıklama 
            OperationResult iletisimAciklama_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.IletisimAciklama);
            AnahtarDeger iletisimAciklama = ((AnahtarDeger)iletisimAciklama_OR.ReturnObject);
            iletisimAciklama.Deger = ayarlar_REQ.IletisimAciklama;
            _anahtarDegerBS.AnahtarGuncelle(iletisimAciklama);


           //İletisim Açıklama Başlık
            OperationResult iletisimAciklamaBaslik_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.IletisimAciklamaBaslik);
            AnahtarDeger iletisimAciklamaBaslik = ((AnahtarDeger)iletisimAciklamaBaslik_OR.ReturnObject);
            iletisimAciklamaBaslik.Deger = ayarlar_REQ.IletisimAciklamaBaslik;
            _anahtarDegerBS.AnahtarGuncelle(iletisimAciklamaBaslik);


            //Faks
            OperationResult faks_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Faks);
            AnahtarDeger faks = ((AnahtarDeger)faks_OR.ReturnObject);
            faks.Deger = ayarlar_REQ.Faks;
            _anahtarDegerBS.AnahtarGuncelle(faks);

            //E-mail
            OperationResult email_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Email);
            AnahtarDeger email = ((AnahtarDeger)email_OR.ReturnObject);
            email.Deger = ayarlar_REQ.Email;
            _anahtarDegerBS.AnahtarGuncelle(email);

            //Adres - ING-TR
            OperationResult adresler_OR = _anahtarDegerBS.AnahtarGetir(Tanimlamalar.Adres);
            List<AnahtarDeger> adresler = (List<AnahtarDeger>)adresler_OR.ReturnObject;
            adresler.FirstOrDefault(x => x.Dil == Dil.Turkce).Deger = ayarlar_REQ.Adres_TR;


            _anahtarDegerBS.AnahtarGuncelle(adresler.FirstOrDefault(x => x.Dil == Dil.Turkce));
            _anahtarDegerBS.AnahtarGuncelle(adresler.FirstOrDefault(x => x.Dil == Dil.Ingilizce));

            //Footer Yazi - ING-TR
            OperationResult footerYazilari_OR = _anahtarDegerBS.AnahtarGetir(Tanimlamalar.FooterYazi);
            List<AnahtarDeger> footerYazilari = (List<AnahtarDeger>)footerYazilari_OR.ReturnObject;
            footerYazilari.FirstOrDefault(x => x.Dil == Dil.Turkce).Deger = ayarlar_REQ.FooterYazi_TR;
            footerYazilari.FirstOrDefault(x => x.Dil == Dil.Ingilizce).Deger = ayarlar_REQ.FooterYazi_EN;

            _anahtarDegerBS.AnahtarGuncelle(footerYazilari.FirstOrDefault(x => x.Dil == Dil.Turkce));
            _anahtarDegerBS.AnahtarGuncelle(footerYazilari.FirstOrDefault(x => x.Dil == Dil.Ingilizce));

            //AnaSayfa Banner - ING-TR
            OperationResult anaSayfaBanner_OR = _anahtarDegerBS.AnahtarGetir(Tanimlamalar.AnaSayfaBanner);
            List<AnahtarDeger> anaSayfaBannerleri = (List<AnahtarDeger>)anaSayfaBanner_OR.ReturnObject;

            anaSayfaBannerleri.FirstOrDefault(x => x.Dil == Dil.Turkce).Deger = JsonConvert.SerializeObject(ayarlar_REQ.AnaSayfaBanner_TR);
            anaSayfaBannerleri.FirstOrDefault(x => x.Dil == Dil.Ingilizce).Deger = JsonConvert.SerializeObject(ayarlar_REQ.AnaSayfaBanner_EN);

            _anahtarDegerBS.AnahtarGuncelle(anaSayfaBannerleri.FirstOrDefault(x => x.Dil == Dil.Turkce));
            _anahtarDegerBS.AnahtarGuncelle(anaSayfaBannerleri.FirstOrDefault(x => x.Dil == Dil.Ingilizce));


            //Description
            OperationResult description_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Description);
            AnahtarDeger description = ((AnahtarDeger)description_OR.ReturnObject);
            description.Deger = ayarlar_REQ.Description;
            _anahtarDegerBS.AnahtarGuncelle(description);

            //MainKeywords
            OperationResult mainkeywords_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.MainKeywords);
            AnahtarDeger mainkeywords = ((AnahtarDeger)mainkeywords_OR.ReturnObject);
            mainkeywords.Deger = ayarlar_REQ.MainKeywords;
            _anahtarDegerBS.AnahtarGuncelle(mainkeywords);

            //Google Map
            OperationResult googleMap_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.GoogleMap);
            AnahtarDeger googleMap = ((AnahtarDeger)googleMap_OR.ReturnObject);
            googleMap.Deger = ayarlar_REQ.GoogleMap;
            _anahtarDegerBS.AnahtarGuncelle(googleMap);

            //Instagram
            OperationResult instagram_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Instagram);
            AnahtarDeger instagram = ((AnahtarDeger)instagram_OR.ReturnObject);
            instagram.Deger = ayarlar_REQ.Instagram;
            _anahtarDegerBS.AnahtarGuncelle(instagram);

            //Instagram
            OperationResult twitter_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Twitter);
            AnahtarDeger twitter = ((AnahtarDeger)twitter_OR.ReturnObject);
            twitter.Deger = ayarlar_REQ.Twitter;
            _anahtarDegerBS.AnahtarGuncelle(twitter);

            //facebook
            OperationResult facebook_OR = _anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.Facebook);
            AnahtarDeger facebook = ((AnahtarDeger)facebook_OR.ReturnObject);
            facebook.Deger = ayarlar_REQ.Facebook;
            _anahtarDegerBS.AnahtarGuncelle(facebook);

            return RedirectToAction("SiteAyarlari");

        }
        #endregion

        #region Slider
        [HttpGet]
        public async Task<IActionResult> Slider()
        {
            List<SliderResim> SliderResimleri = (List<SliderResim>)(await _sliderResimBS.SliderResimleriGetir()).ReturnObject;
            ViewBag.SliderResimleri = SliderResimleri.OrderBy(x => x.Dil).ThenBy(x => x.ResimSira).ToList();

            return View(new SliderResim());
        }
        [HttpPost]
        public async Task<IActionResult> Slider([FromForm]SliderResim sliderResim)
        {
            List<SliderResim> SliderResimleri = (List<SliderResim>)(await _sliderResimBS.SliderResimleriGetir()).ReturnObject;
            ViewBag.SliderResimleri = SliderResimleri.OrderBy(x => x.SliderYeri).ThenBy(x => x.Dil).ThenBy(x => x.ResimSira).ToList();

            if (!ModelState.IsValid)
            {
                return View(sliderResim);
            }

            await _sliderResimBS.SliderResimEkle(sliderResim);

            return RedirectToAction("Slider");
        }
        [HttpPost]
        public async Task<JsonResult> SliderSil(int sliderId)
        {
            if (sliderId == 0)
            {
                return Json(new Genel_RES()
                {
                    BasariliMi = false,
                    Mesaj = "Slider resmi bulunamadı"
                });
            }

            OperationResult sliderResimSil_OR = await _sliderResimBS.SliderResimSil(sliderId);
            if (!sliderResimSil_OR.IsSuccess)
            {
                Genel_RES genel_RES = new Genel_RES()
                {
                    BasariliMi = false
                };

                switch (sliderResimSil_OR.Message)
                {
                    case Enums.MesajKodu.SliderBulunamadi:
                        genel_RES.Mesaj = "Slider bulunamadı";
                        break;
                    case Enums.MesajKodu.BeklenmedikHata:
                    default:
                        genel_RES.Mesaj = "Beklenmedik bir hata meydana geldi";
                        break;
                }

                return Json(genel_RES);
            }

            return Json(new Genel_RES()
            {
                BasariliMi = true,
                Mesaj = "Slider resmi başarıyla silindi"
            });

        }
        [HttpPost]
        public async Task<IActionResult> SliderDuzenle([FromForm]SliderResim sliderResim)
        {
            List<SliderResim> SliderResimleri = (List<SliderResim>)(await _sliderResimBS.SliderResimleriGetir()).ReturnObject;
            ViewBag.SliderResimleri = SliderResimleri.OrderBy(x => x.SliderYeri).ThenBy(x => x.Dil).ThenBy(x => x.ResimSira).ToList();

            if (!ModelState.IsValid)
            {
                return View(sliderResim);

            }

            await _sliderResimBS.SliderResimGuncelle(sliderResim);
            return RedirectToAction("Slider");
        }

        #endregion

    }
}