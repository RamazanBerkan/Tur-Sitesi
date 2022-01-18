using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArgedeSP.Contracts.DTO.BlogKategori.Req;
using ArgedeSP.Contracts.DTO.BlogKategori.Res;
using ArgedeSP.Contracts.DTO.Bloglar.Req;
using ArgedeSP.Contracts.DTO.Bloglar.Res;
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
    public class BloglarController : Controller
    {
        public IBlogKategoriBS _blogKategoriBS;
        public IBlogBS _blogBS;
        public BloglarController(IBlogKategoriBS blogKategoriBS, IBlogBS blogBS)
        {
            _blogKategoriBS = blogKategoriBS;
            _blogBS = blogBS;
        }
        [HttpGet]
        public IActionResult Bloglar()
        {
            return View();
        }
        [HttpPost]
        public JsonResult BlogGetir()
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
            string baslik = dict["columns[1][search][value]"];
            string bkategori = dict["columns[3][search][value]"];
            //string seoUrl = dict["columns[4][search][value]"];
            int dil = 0;
            int.TryParse(dict["columns[4][search][value]"], out dil);


            OperationResult veriListeleme_OR = _blogBS.BloglariSayfala(sayfa2, sayfaBoyutu, id, baslik, (Dil)dil, "", bkategori);

            if (veriListeleme_OR.IsSuccess)
            {
                VeriListeleme veriListeleme = (VeriListeleme)veriListeleme_OR.ReturnObject;

                return Json(new
                {
                    iTotalRecords = veriListeleme.ToplamVeri,
                    iTotalDisplayRecords = veriListeleme.ToplamVeri,
                    sEcho = 0,
                    sColumns = "",
                    aaData = ((List<Blog>)veriListeleme.Veri).Select(x => new
                    {
                        x.Id,
                        Ad = x.Baslik,
                        x.ArkaPlanResim,
                        x.Resim,
                        x.Baslik,
                        x.BlogEtiket,
                        x.KisaAciklama,
                        x.UzunAciklama,
                        OlusturmaTarihi = x.OlusturmaTarihi.ToString("f"),
                        x.SeoUrl,
                        BlogKategori = x.BlogKategori.BlogKategoriAdi,
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
        public async Task<IActionResult> BlogEkle()
        {
            return View(new BlogEkle_RES()
            {
                BlogEkle_REQ = new BlogEkle_REQ(),
                BlogKategorileri = new List<BlogKategori>()
            });
        }
        [HttpPost]
        public async Task<IActionResult> BlogEkle([FromForm]BlogEkle_REQ blogEkle_REQ)
        {
            if (!ModelState.IsValid)
            {
                return View(new BlogEkle_RES()
                {
                    BlogEkle_REQ = blogEkle_REQ,
                });
            }

            OperationResult blogEkle_OR = await _blogBS.BlogEkle(blogEkle_REQ);
            if (blogEkle_OR.IsSuccess)
            {
                TempData["Basarili"] = "Blog başarıyla eklendi";
                return RedirectToAction("BlogDuzenle", new { blogId = ((Blog)blogEkle_OR.ReturnObject).Id });
            }

            switch (blogEkle_OR.Message)
            {
                case MesajKodu.SeoUrlZatenVar:
                    ModelState.AddModelError("BlogEkle_REQ.SeoUrl", "Bu seo url zaten kayıtlı, Lütfen değiştiriniz");
                    break;
                default:
                    TempData["Hata"] = "Blog eklenemedi";
                    return RedirectToAction("Bloglar");
            }

            return View(new BlogEkle_RES()
            {
                BlogEkle_REQ = blogEkle_REQ,
            });


        }
        [HttpPost]
        public async Task<JsonResult> BlogSil(int blogId)
        {
            if (blogId == 0)
            {
                return Json(new Genel_RES()
                {
                    BasariliMi = false,
                    Mesaj = "Blog bulunamadı",
                });
            }

            OperationResult blogSil_OR = await _blogBS.BlogSil(blogId);

            if (blogSil_OR.IsSuccess)
            {
                return Json(new Genel_RES()
                {
                    BasariliMi = true,
                    Mesaj = "Blog başarıyla silindi."
                });
            }

            Genel_RES sonuc = new Genel_RES();
            sonuc.BasariliMi = false;

            switch (blogSil_OR.Message)
            {
                case MesajKodu.BlogBulunamadı:
                    sonuc.Mesaj = "Blog bulunamadi";
                    break;
                default:
                    sonuc.Mesaj = "Beklenmedik bir hata meydana geldi";
                    break;
            }

            return Json(sonuc);
        }
        [HttpGet]
        public async Task<IActionResult> BlogDuzenle(int blogId)
        {
            if (blogId == 0)
            {
                TempData["Hata"] = "Blog bulunamadi";
                return RedirectToAction("Bloglar");
            }

            OperationResult blog_OR = await _blogBS.BlogGetirIdIle(blogId);
            Blog blog = (Blog)blog_OR.ReturnObject;
            if (!blog_OR.IsSuccess || blog == null)
            {
                TempData["Hata"] = "Blog bulunamadi";
                return RedirectToAction("Bloglar");
            }

            List<BlogKategori> blogKategorileri = (List<BlogKategori>)(_blogKategoriBS.BlogKategorileriGetir(blog.Dil, int.MaxValue)).ReturnObject;

            return View(new BlogEkle_RES()
            {
                BlogKategorileri = blogKategorileri,
                BlogEkle_REQ = new BlogEkle_REQ()
                {
                    Baslik = blog.Baslik,
                    Id = blog.Id,
                    KisaAciklama = blog.KisaAciklama,
                    ArkaPlanResim = blog.ArkaPlanResim,
                    BlogEtiket = blog.BlogEtiket,
                    Resim = blog.Resim,
                    Ses=blog.Ses,
                    SeoUrl = blog.SeoUrl,
                    UzunAciklama = blog.UzunAciklama,
                    BlogKategoriId = blog.BlogKategoriId,
                    Dil = blog.Dil,

                }
            });

        }
        [HttpPost]
        public async Task<IActionResult> BlogDuzenle([FromForm]BlogEkle_REQ blogEkle_REQ)
        {
            if (!ModelState.IsValid)
            {
                return View(new BlogEkle_RES()
                {
                    BlogEkle_REQ = blogEkle_REQ,
                });
            }

            if (blogEkle_REQ.Id == 0)
            {
                TempData["Hata"] = "Blog Bulunamadı";
                return RedirectToAction("Bloglar");
            }

            OperationResult blogEkle_OR = await _blogBS.BlogGuncelle(blogEkle_REQ);
            if (blogEkle_OR.IsSuccess)
            {
                TempData["Basarili"] = "Blog başarıyla güncellendi";
                return RedirectToAction("BlogDuzenle", new { blogId = blogEkle_REQ.Id });
            }

            switch (blogEkle_OR.Message)
            {
                case MesajKodu.SeoUrlZatenVar:
                    ModelState.AddModelError("BlogEkle_REQ.SeoUrl", "Bu seo url zaten kayıtlı, Lütfen değiştiriniz");
                    break;
                case MesajKodu.BlogGuncellendi:
                default:
                    TempData["Basarili"] = "Blog başarıyla güncellendi";
                    return RedirectToAction("blogDuzenle", new { blogId = blogEkle_REQ.Id });
            }

            return View(new BlogEkle_RES()
            {
                BlogEkle_REQ = blogEkle_REQ,
            });
        }




        [HttpGet]
        public IActionResult BlogKategoriler()
        {
            return View();
        }
        [HttpPost]
        public JsonResult BlogKategorileriGetir()
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
            string blogkategoriadi = dict["columns[1][search][value]"];
            int dil = 0;
            int.TryParse(dict["columns[2][search][value]"], out dil);

            OperationResult veriListeleme_OR = _blogKategoriBS.BlogKategorileriniSayfala(sayfa2, sayfaBoyutu, id, blogkategoriadi, "", (Dil)dil);

            if (veriListeleme_OR.IsSuccess)
            {
                VeriListeleme veriListeleme = (VeriListeleme)veriListeleme_OR.ReturnObject;

                return Json(new
                {
                    iTotalRecords = veriListeleme.ToplamVeri,
                    iTotalDisplayRecords = veriListeleme.ToplamVeri,
                    sEcho = 0,
                    sColumns = "",
                    aaData = ((List<BlogKategori>)veriListeleme.Veri).Select(x => new
                    {
                        x.Id,
                        x.BlogKategoriAdi,
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
        public IActionResult BlogKategoriEkle()
        {
            return View(new BlogKategoriEkle_RES()
            {
                BlogKategoriEkle_REQ = new BlogKategoriEkle_REQ()
            });
        }
        [HttpPost]
        public async Task<IActionResult> BlogKategoriEkle([FromForm]BlogKategoriEkle_REQ blogKategoriEkle_REQ)
        {
            if (!ModelState.IsValid)
            {
                return View(new BlogKategoriEkle_RES()
                {
                    BlogKategoriEkle_REQ = blogKategoriEkle_REQ,
                });
            }

            OperationResult blogKategoriEkle_OR = await _blogKategoriBS.BlogKategorisiEkle(blogKategoriEkle_REQ);
            if (blogKategoriEkle_OR.IsSuccess)
            {
                TempData["Basarili"] = "Kategori başarıyla eklendi";
                return RedirectToAction("BlogKategoriDuzenle", new { blogkategoriId = ((BlogKategori)blogKategoriEkle_OR.ReturnObject).Id });
            }

            switch (blogKategoriEkle_OR.Message)
            {
                case MesajKodu.SeoUrlZatenVar:
                    ModelState.AddModelError("BlogKategoriEkle_REQ.SeoUrl", "Bu seo url zaten kayıtlı, Lütfen değiştiriniz");
                    break;
                default:
                    TempData["Basarili"] = "Kategori başarıyla eklendi";
                    return RedirectToAction("BlogKategoriEkle");
            }

            return View(new BlogKategoriEkle_RES()
            {
                BlogKategoriEkle_REQ = blogKategoriEkle_REQ,
            });


        }
        [HttpPost]
        public async Task<JsonResult> BlogKategoriSil(int blogkategoriId)
        {
            if (blogkategoriId == 0)
            {
                return Json(new Genel_RES()
                {
                    BasariliMi = false,
                    Mesaj = "Kategori bulunamadı",
                });
            }

            OperationResult blogkategoriSil_OR = await _blogKategoriBS.BlogKategoriSil(blogkategoriId);

            if (blogkategoriSil_OR.IsSuccess)
            {
                return Json(new Genel_RES()
                {
                    BasariliMi = true,
                    Mesaj = "Kategori başarıyla silindi."
                });
            }

            Genel_RES sonuc = new Genel_RES();
            sonuc.BasariliMi = false;

            switch (blogkategoriSil_OR.Message)
            {
                case MesajKodu.BlogKategoriBulunamadı:
                    sonuc.Mesaj = "Kategori bulunamadi";
                    break;
                default:
                    sonuc.Mesaj = "Beklenmedik bir hata meydana geldi";
                    break;
            }

            return Json(sonuc);
        }
        [HttpGet]
        public async Task<IActionResult> BlogKategoriDuzenle(int blogkategoriId)
        {
            if (blogkategoriId == 0)
            {
                TempData["Hata"] = "Kategori bulunamadi";
                return RedirectToAction("BlogKategoriler");
            }

            OperationResult blogkategori_OR = await _blogKategoriBS.BlogKategorisiGetirIdIle(blogkategoriId);
            BlogKategori blogKategori = (BlogKategori)blogkategori_OR.ReturnObject;
            if (!blogkategori_OR.IsSuccess || blogKategori == null)
            {
                TempData["Hata"] = "Kategori bulunamadi";
                return RedirectToAction("BlogKategoriler");
            }

            return View(new BlogKategoriEkle_RES()
            {
                BlogKategoriEkle_REQ = new BlogKategoriEkle_REQ()
                {
                    BlogKategoriAdi = blogKategori.BlogKategoriAdi,
                    ArkaPlanResim = blogKategori.ArkaPlanResim,
                    Id = blogKategori.Id,
                    KisaAciklama = blogKategori.KisaAciklama,
                    Resim = blogKategori.Resim,
                    SeoUrl = blogKategori.SeoUrl,
                    UzunAciklama = blogKategori.UzunAciklama,
                    Dil = blogKategori.Dil,
                    AnaDilcesi = blogKategori.AnaDilcesi
                }
            });

        }
        [HttpPost]
        public async Task<IActionResult> BlogKategoriDuzenle([FromForm]BlogKategoriEkle_REQ blogKategoriEkle_REQ)
        {
            if (!ModelState.IsValid)
            {
                return View(new BlogKategoriEkle_RES()
                {
                    BlogKategoriEkle_REQ = blogKategoriEkle_REQ,
                });
            }

            if (blogKategoriEkle_REQ.Id == 0)
            {
                TempData["Hata"] = "Kategori Bulunamadı";
                return RedirectToAction("BlogKategoriler");
            }

            OperationResult blogKategoriEkle_OR = await _blogKategoriBS.BlogKategoriGuncelle(blogKategoriEkle_REQ);
            if (blogKategoriEkle_OR.IsSuccess)
            {
                TempData["Basarili"] = "Kategori başarıyla güncellendi";
                return RedirectToAction("BlogKategoriDuzenle", new { blogkategoriId = blogKategoriEkle_REQ.Id });
            }

            switch (blogKategoriEkle_OR.Message)
            {
                case MesajKodu.SeoUrlZatenVar:
                    ModelState.AddModelError("BlogKategoriEkle_REQ.SeoUrl", "Bu seo url zaten kayıtlı, Lütfen değiştiriniz");
                    break;
                case MesajKodu.BlogKategoriGuncellendi:
                default:
                    TempData["Basarili"] = "Kategori başarıyla güncellendi";
                    return RedirectToAction("BlogKategoriDuzenle", new { blogkategoriId = blogKategoriEkle_REQ.Id });
            }

            return View(new BlogKategoriEkle_RES()
            {
                BlogKategoriEkle_REQ = blogKategoriEkle_REQ,
            });
        }


        [HttpPost]
        public JsonResult BlogDilKategorileri(int dil)
        {
            OperationResult kategoriDil_OR = _blogKategoriBS.BlogKategorileriGetir((Dil)dil, int.MaxValue);
            List<BlogKategori> projeKategorileri = (List<BlogKategori>)kategoriDil_OR.ReturnObject;

            return Json(new Genel_RES()
            {
                BasariliMi = true,
                Model = projeKategorileri.Select(x => new
                {
                    KategoriId = x.Id,
                    KategoriAdi = x.BlogKategoriAdi + " -- (" + x.AnaDilcesi + ")"
                }).ToList()
            });
        }
    }
}