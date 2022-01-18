using System;
using System.Collections.Generic;
using System.Linq;
using ArgedeSP.Contracts.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using static ArgedeSP.Contracts.Models.Common.Enums;
using System.Security.Claims;
using ArgedeSP.DAL.DataContext;
using ArgedeSP.Contracts.Models.Common;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ArgedeSP.DAL
{
    public class ArgedeSPDataInitializer
    {
        private RoleManager<Rol> _roleManager;
        private UserManager<Kullanici> _userManager;

        private Kullanici admin;

        private List<Dil> Diller = new List<Dil>()
        {
            Dil.Turkce,
            Dil.Ingilizce,
        };


        public ArgedeSPDataInitializer(
            UserManager<Kullanici> userManager,
            RoleManager<Rol> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task EnsurePopulatedAsync(IServiceProvider app)
        {
            var context = app.GetRequiredService<ArgedeSPContext>();
              

            if (!(context.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
            {
                context.Database.Migrate();
            }


            if (!context.Kullanicilar.Any())
            {
                await KullaniciEkle(context, 0);
                HizmetEkle(context, 3);
                OnHizmetEkle(context, 3);
                UrunKategoriEkle(context, 2);
                UrunEkle(context, 2);
                UrunGaleriEkle(context, 4);
                UrunDokumanlariEkle(context, 4);

                BlogKategoriEkle(context, 3);
                BlogEkle(context, 3);
                HaberleriEkle(context, 10);
                ReferansEkle(context, 4);
                SliderResimEkle(context, 3);
                AnahtarDegerleriEkle(context);
            }
        }

        private async Task KullaniciEkle(ArgedeSPContext context, int adet)
        {
            Random rnd = new Random();
            var testAdmin = await _userManager.FindByNameAsync("admin");
            if (testAdmin == null)
            {
                if (!(await _roleManager.RoleExistsAsync("Admin")))
                {
                    var adminRole = new Rol()
                    {
                        Name = "Admin",
                        NormalizedName = "ADMIN"
                    };
                    await _roleManager.CreateAsync(adminRole);
                    await _roleManager.AddClaimAsync(adminRole, new Claim("IsAdmin", "true"));
                }

                testAdmin = new Kullanici()
                {
                    UserName = "admin",
                    Email = "yusuf@argede.com.tr",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    OlusturmaTarihi = DateTime.Now,
                    ProfilResmi = "/img/profil-resimleri/Profil.png",
                    Ad = "Admin",
                    Soyad = "ADMIN",
                    Durum = Durum.Aktif

                };

                var adminResult = await _userManager.CreateAsync(testAdmin, "123456+");
                var adminRolResult = await _userManager.AddToRoleAsync(testAdmin, "Admin");
                await _userManager.AddClaimAsync(testAdmin, new Claim("AdSoyad", testAdmin.Ad + " " + testAdmin.Soyad));
                await _userManager.AddClaimAsync(testAdmin, new Claim("ProfilResmi", testAdmin.ProfilResmi));
                await _userManager.AddClaimAsync(testAdmin, new Claim("KullaniciId", testAdmin.Id));


                if (!adminResult.Succeeded || !adminRolResult.Succeeded)
                {
                    throw new InvalidOperationException("kullanıcı yaratmada sorun var!");
                }

            }

            var kullaniciRol = new Rol()
            {
                Name = "Kullanici",
                NormalizedName = "KULLANICI"
            };
            await _roleManager.CreateAsync(kullaniciRol);

            for (int i = 0; i < adet; i++)
            {

                Kullanici testKullanici = new Kullanici()
                {
                    UserName = "kullanici" + (i + 1),
                    Email = i == 0 ? "yusuf@argede.com.tr" : "yusuf" + (i + 1) + "@argede.com.tr",//
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ProfilResmi = "/img/profil-resimleri/profil.png",
                    PhoneNumber = "05312640852",
                    Durum = i % 2 == 0 ? Durum.Aktif : Durum.Pasif,
                    Ad = "Ramazan",
                    Soyad = "Tüt",
                    TCKN = "55555555555",
                    OlusturmaTarihi = DateTime.Now
                };



                var testKullaniciResult = await _userManager.CreateAsync(testKullanici, "123456");
                var testKullaniciRolResult = await _userManager.AddToRoleAsync(testKullanici, "Kullanici");
            }

            admin = context.Kullanicilar.FirstOrDefault(x => x.UserName == "admin");
        }
        private void UrunKategoriEkle(ArgedeSPContext context, int adet)
        {

            for (int i = 0; i < adet; i++)
            {
                UrunKategori urunKategori_TR = new UrunKategori()
                {
                    Ad = "Ürün Kategori " + i,
                    ArkaPlanResim = "/img/urun-kategori-resimleri/test-arkaplan.jpg",
                    KisaAciklama = "Kısa açıklama yazısı",
                    OlusturmaTarihi = DateTime.Now,
                    Resim = "/img/urun-kategori-resimleri/test-kategori.jpg",
                    UzunAciklama = "Uzuuuuuuuuun açıııııııııııklama yazıııııııııısııııııııı",
                    UstId = null,
                    SeoUrl = "urun-kategori-" + i,
                    Dil = Dil.Turkce,
                    AnaDilcesi = "",
                };

                context.UrunKategorileri.Add(urunKategori_TR);

                UrunKategori urunKategori_EN = new UrunKategori()
                {
                    Ad = "Product Category " + i,
                    ArkaPlanResim = "/img/urun-kategori-resimleri/test-arkaplan.jpg",
                    KisaAciklama = "Short Description",
                    OlusturmaTarihi = DateTime.Now,
                    Resim = "/img/urun-kategori-resimleri/test-kategori.jpg",
                    UzunAciklama = "long-description",
                    UstId = null,
                    SeoUrl = "product-category-" + i,
                    Dil = Dil.Ingilizce,
                    AnaDilcesi = "Product Category " + i,
                };

                context.UrunKategorileri.Add(urunKategori_EN);
            }

            context.SaveChanges();


            List<UrunKategori> urunKategorileri = context.UrunKategorileri.Where(x => x.UstId == null && x.Dil == Dil.Turkce).ToList();

            foreach (var anaKategori in urunKategorileri)
            {

                for (int i = 0; i < 2; i++)
                {
                    UrunKategori urunKategori_TR = new UrunKategori()
                    {
                        Ad = "Ürün Alt Kategori " + i,
                        ArkaPlanResim = "/img/urun-kategori-resimleri/test-arkaplan.jpg",
                        KisaAciklama = "Kısa açıklama yazısı",
                        OlusturmaTarihi = DateTime.Now,
                        Resim = "/img/urun-kategori-resimleri/test-kategori.jpg",
                        UzunAciklama = "Uzuuuuuuuuun açıııııııııııklama yazıııııııııısııııııııı",
                        UstId = anaKategori.Id,
                        SeoUrl = "urun-alt-kategori-" + i,
                        Dil = Dil.Turkce,
                        AnaDilcesi = "",
                    };

                    context.UrunKategorileri.Add(urunKategori_TR);
                }

            }

            context.SaveChanges();

            urunKategorileri = context.UrunKategorileri.Where(x => x.UstId == null && x.Dil == Dil.Ingilizce).ToList();


            foreach (var anaKategori in urunKategorileri)
            {

                for (int i = 0; i < 2; i++)
                {
                    UrunKategori urunKategori_EN = new UrunKategori()
                    {
                        Ad = "Product Sub Category " + i,
                        ArkaPlanResim = "/img/urun-kategori-resimleri/test-arkaplan.jpg",
                        KisaAciklama = "Short Description",
                        OlusturmaTarihi = DateTime.Now,
                        Resim = "/img/urun-kategori-resimleri/test-kategori.jpg",
                        UzunAciklama = "long-description",
                        UstId = anaKategori.Id,
                        SeoUrl = "product-sub-category-" + i,
                        Dil = Dil.Ingilizce,
                        AnaDilcesi = "Product Category " + i,
                    };

                    context.UrunKategorileri.Add(urunKategori_EN);
                }

            }

            context.SaveChanges();

        }
        private void UrunEkle(ArgedeSPContext context, int adet)
        {
            Random rnd = new Random();

            List<UrunKategori> urunKateogorileri = context.UrunKategorileri.Where(x => x.UstId != null && x.Dil == Dil.Turkce).ToList();
            int sayac = 0;
            for (int i = 0; i < urunKateogorileri.Count; i++)
            {
                for (int j = 0; j < adet; j++)
                {
                    sayac++;
                    Urun urun_TR = new Urun()
                    {
                        Durum = Durum.Aktif,
                        KisaAciklama = "Kısa açıklama yazısı " + sayac,
                        OlusturmaTarihi = DateTime.Now,
                        Resim = "/img/urun-resimleri/test-urun.png",
                        SeoUrl = "urun-adi-" + sayac,
                        UrunAdi = "Ürün Adı " + sayac,
                        Keywords = "Keywords " + sayac,
                        //UrunModel = "Mongoose XC Aluminum Hardtail",

                        UrunKategori = urunKateogorileri[i],
                        UrunKodu = "urun-kodu-" + sayac,
                        UzunAciklama = "Uzuk açıklama yazısı",
                        NasilCalisir = "Nasıl Çalışır yazisi " + sayac,
                        //Fiyat = rnd.Next(50, 1000),
                        TeknikOzellikler = "Teknik Ozellikler  " + sayac,
                        Dil = Dil.Turkce
                    };

                    context.Urunler.Add(urun_TR);
                }
            }

            context.SaveChanges();

            urunKateogorileri = context.UrunKategorileri.Where(x => x.UstId != null && x.Dil == Dil.Ingilizce).ToList();
            sayac = 0;
            for (int i = 0; i < urunKateogorileri.Count; i++)
            {
                for (int j = 0; j < adet; j++)
                {
                    sayac++;
                    Urun urun_EN = new Urun()
                    {
                        Durum = Durum.Aktif,
                        KisaAciklama = "Short description text" + sayac,
                        OlusturmaTarihi = DateTime.Now,
                        Resim = "/img/urun-resimleri/test-urun.png",
                        SeoUrl = "product-name-" + sayac,
                        UrunAdi = "Product Name " + sayac,
                        Keywords = " Keywords " + sayac,
                        UrunKategori = urunKateogorileri[i],
                        UrunKodu = "urun-kodu-" + sayac,
                        UzunAciklama = "Long description text",
                        NasilCalisir = "How will working  " + sayac,
                        //Fiyat = rnd.Next(50, 1000),
                        TeknikOzellikler = "Technical Future " + sayac,
                        Dil = Dil.Ingilizce
                    };

                    context.Urunler.Add(urun_EN);
                }
            }

            context.SaveChanges();

        }
        private void UrunGaleriEkle(ArgedeSPContext context, int adet)
        {
            Random rnd = new Random();
            List<Urun> urunler = context.Urunler.ToList();
            for (int i = 0; i < urunler.Count; i++)
            {
                for (int j = 0; j < adet; j++)
                {
                    UrunResimGaleri urunResimGaleri = new UrunResimGaleri()
                    {
                        OlusturmaTarihi = DateTime.Now,
                        ResimUrl = "/img/urun-resimleri/urun-galeri-resmi.png",
                        Urun = urunler[i],
                    };

                    context.UrunResimGalerileri.Add(urunResimGaleri);
                }
            }

            context.SaveChanges();
        }

        private void UrunDokumanlariEkle(ArgedeSPContext context, int adet)
        {
            Random rnd = new Random();
            List<Urun> urunler = context.Urunler.ToList();
            for (int i = 0; i < urunler.Count; i++)
            {
                for (int j = 0; j < adet; j++)
                {
                    UrunDokumani urunDokumani = new UrunDokumani()
                    {
                        OlusturmaTarihi = DateTime.Now,
                        DokumanUrl = "/dokumanlar/dokuman1.pdf",
                        Urun = urunler[i],
                    };

                    context.UrunDokumanlari.Add(urunDokumani);
                }
            }

            context.SaveChanges();
        }
        private void HizmetEkle(ArgedeSPContext context, int adet)
        {
            for (int j = 0; j < adet; j++)
            {
                Hizmet hizmet = new Hizmet()
                {
                    ArkaPlanResim = "/img/hizmet-resimleri/test-arkaplan.jpg",
                    KisaAciklama = "Hizmet Kısa Açıklama Örnek." + j,
                    HizmetAdi = "Hİzmet Adı" + j,
                    Resim = "/img/hizmet-resimleri/test-hizmet.jpg",
                    UzunAciklama = "Hİzmet Örnek Uzun Açıklaması." + j,
                    SeoUrl = "hizmet-adi-" + j,
                    Dil = Dil.Turkce
                };

                context.Hizmetler.Add(hizmet);

                context.SaveChanges();
            }
            for (int j = 0; j < adet; j++)
            {
                Hizmet hizmet = new Hizmet()
                {
                    ArkaPlanResim = "/img/hizmet-resimleri/test-arkaplan.jpg",
                    KisaAciklama = "Service Short Description Example." + j,
                    HizmetAdi = "Service Name" + j,
                    Resim = "/img/hizmet-resimleri/test-hizmet.jpg",
                    UzunAciklama = "Service Example Long Description." + j,
                    SeoUrl = "service-name-" + j,
                    Dil = Dil.Ingilizce
                };

                context.Hizmetler.Add(hizmet);

                context.SaveChanges();
            }
        }
        private void OnHizmetEkle(ArgedeSPContext context, int adet)
        {
            for (int j = 0; j < adet; j++)
            {
                OnHizmet onhizmet = new OnHizmet()
                {
                    ArkaPlanResim = "/img/hizmet-resimleri/test-arkaplan.jpg",
                    KisaAciklama = "Hizmet Kısa Açıklama Örnek." + j,
                    OnHizmetAdi = "Öne Çıkan Hİzmet Adı" + j,
                    Resim = "/img/hizmet-resimleri/test-hizmet.jpg",
                    UzunAciklama = "Hİzmet Örnek Uzun Açıklaması." + j,
                    SeoUrl = "hizmet-adi-" + j,
                    Dil = Dil.Turkce
                };

                context.OnHizmetler.Add(onhizmet);

                context.SaveChanges();
            }
            for (int j = 0; j < adet; j++)
            {
                OnHizmet onhizmet = new OnHizmet()
                {
                    ArkaPlanResim = "/img/hizmet-resimleri/test-arkaplan.jpg",
                    KisaAciklama = "Service Short Description Example." + j,
                    OnHizmetAdi = "Service Name" + j,
                    Resim = "/img/hizmet-resimleri/test-hizmet.jpg",
                    UzunAciklama = "Service Example Long Description." + j,
                    SeoUrl = "service-name-" + j,
                    Dil = Dil.Ingilizce
                };

                context.OnHizmetler.Add(onhizmet);

                context.SaveChanges();
            }
        }
        private void ReferansEkle(ArgedeSPContext context, int adet)
        {
            for (int i = 0; i < adet; i++)
            {
                Referans referans = new Referans()
                {
                    ArkaPlanResmi = "/img/referans-resimleri/test-referans-arkaplan.jpg",
                    KisaAciklama = "Referans kısa açıklaması " + i,
                    ReferansAdi = "Referans Adı " + i,
                    Resim = "/img/referans-resimleri/test-referans.jpg",
                    UzunAciklama = "Referans uzun açıklama yazısı " + i,
                    SeoUrl = "referans-adi" + i
                };

                context.Referanslar.Add(referans);
            }
            context.SaveChanges();
        }
        private void BlogKategoriEkle(ArgedeSPContext context, int adet)
        {
            for (int i = 0; i < adet; i++)
            {
                BlogKategori blogKategori = new BlogKategori()
                {
                    ArkaPlanResim = "/img/blog-kategori-resimleri/test-blog-kategori-arkaplan.jpg",
                    KisaAciklama = "Blog Kategori kısa açıklaması ",
                    BlogKategoriAdi = "B.Kategori Adı " + i,
                    Resim = "/img/blog-kategori-resimleri/test-blog-kategori.jpg",
                    UzunAciklama = "Blog Kategori uzun açıklama yazısı " + i,
                    SeoUrl = "blog-kategori-adi" + i,
                    Dil = Dil.Turkce,
                };

                context.BlogKategorileri.Add(blogKategori);
            }

            for (int i = 0; i < adet; i++)
            {
                BlogKategori blogKategori = new BlogKategori()
                {
                    ArkaPlanResim = "/img/blog-kategori-resimleri/test-blog-kategori-arkaplan.jpg",
                    KisaAciklama = "Blog Category short description",
                    BlogKategoriAdi = "B.Category Name " + i,
                    Resim = "/img/blog-kategori-resimleri/test-blog-kategori.jpg",
                    UzunAciklama = "Blog Category long description post",
                    SeoUrl = "blog-category-name" + i,
                    Dil = Dil.Ingilizce,
                    AnaDilcesi = "Kategori Adı"
                };

                context.BlogKategorileri.Add(blogKategori);
            }

            context.SaveChanges();
        }
        private void BlogEkle(ArgedeSPContext context, int adet)
        {
            List<BlogKategori> blogKategorileri_tr = context.BlogKategorileri.Where(x => x.Dil == Dil.Turkce).ToList();
            int blogSayac = 1;
            for (int i = 0; i < blogKategorileri_tr.Count; i++)
            {
                for (int j = 0; j < adet; j++)
                {
                    Blog blog = new Blog()
                    {
                        ArkaPlanResim = "/img/blog-resimleri/test-blog-arkaplan.jpg",
                        Resim = "/img/blog-resimleri/test-blog.jpg",
                        KisaAciklama = "Blog Kısa Açıklama .",
                        OlusturmaTarihi = DateTime.Now,
                        Baslik = "Blog Başlığı " + blogSayac,
                        BlogEtiket = "#blog, #etiket, #default",
                        UzunAciklama = "Blog Uzun Açıklama",
                        BlogKategori = blogKategorileri_tr[i],
                        SeoUrl = "blog-adi-" + i,
                        Dil = Dil.Turkce
                    };

                    context.Bloglar.Add(blog);

                    blogSayac++;
                }
            }
            blogSayac = 1;
            List<BlogKategori> blogKategorileri_en = context.BlogKategorileri.Where(x => x.Dil == Dil.Ingilizce).ToList();
            for (int i = 0; i < blogKategorileri_en.Count; i++)
            {
                for (int j = 0; j < adet; j++)
                {
                    Blog blog = new Blog()
                    {
                        ArkaPlanResim = "/img/blog-resimleri/test-blog-arkaplan.jpg",
                        Resim = "/img/blog-resimleri/test-blog.jpg",
                        KisaAciklama = "Blog Short Description.",
                        OlusturmaTarihi = DateTime.Now,
                        Baslik = "Blog Title " + blogSayac,
                        BlogEtiket = "#blog, #etiket, #default",
                        UzunAciklama = "Blog Long Description",
                        BlogKategori = blogKategorileri_en[i],
                        SeoUrl = "blog-title-" + i,
                        Dil = Dil.Ingilizce
                    };

                    context.Bloglar.Add(blog);

                    blogSayac++;

                }
            }

            context.SaveChanges();
        }
        private void HaberleriEkle(ArgedeSPContext context, int adet)
        {
            for (int i = 0; i < adet; i++)
            {
                Haber haber = new Haber()
                {
                    Baslik = "Haber Başlığı " + (i + 1),
                    Dil = Dil.Turkce,
                    KisaAciklama = "Haber Kısa Açıklaması",
                    OlusturmaTarihi = DateTime.Now,
                    Resim = "/img/haber-resimleri/resim-yok1.png",
                    UzunAciklama = "Haber uzun açıklama yazısı"
                };
                context.Haberler.Add(haber);
            }

            for (int i = 0; i < adet; i++)
            {
                Haber haber = new Haber()
                {
                    Baslik = "News Title " + (i + 1),
                    Dil = Dil.Ingilizce,
                    KisaAciklama = "News Short Description",
                    OlusturmaTarihi = DateTime.Now,
                    Resim = "/img/haber-resimleri/resim-yok1.png",
                    UzunAciklama = "News Long Description"
                };
                context.Haberler.Add(haber);
            }

            context.SaveChanges();
        }
        private void SliderResimEkle(ArgedeSPContext context, int adet)
        {

            for (int i = 0; i < adet; i++)
            {
                SliderResim sliderResim = new SliderResim()
                {
                    UstBaslik = "Üst Baslık " + i,
                    AltBaslik = "Alt Başlık" + i,
                    OlusturmaTarihi = DateTime.Now,
                    ResimSira = i,
                    ResimUrl = "/img/slider-resimleri/slider-" + (i + 1) + ".png",
                    Link = "test",
                    Dil = Dil.Turkce
                };

                context.SliderResimleri.Add(sliderResim);
            }

            for (int i = 0; i < adet; i++)
            {
                SliderResim sliderResim = new SliderResim()
                {
                    UstBaslik = "Top Title " + i,
                    AltBaslik = "Bottom Title" + i,
                    OlusturmaTarihi = DateTime.Now,
                    ResimSira = i,
                    ResimUrl = "/img/slider-resimleri/slider-" + (i + 1) + ".png",

                    Link = "test",
                    Dil = Dil.Ingilizce
                };

                context.SliderResimleri.Add(sliderResim);
            }

            context.SaveChanges();
        }
        private void AnahtarDegerleriEkle(ArgedeSPContext context)
        {
            /////Proje Adı
            AnahtarDeger projeAdi_TR = new AnahtarDeger()
            {
                Anahtar = Tanimlamalar.ProjeAdi,
                Deger = "MNS",
                GorunenAd = "MNS",
                Dil = Dil.Turkce
            };

            AnahtarDeger title_TR = new AnahtarDeger()
            {
                Anahtar = Tanimlamalar.Title,
                Deger = "MNS",
                GorunenAd = "MNS",
                Dil = Dil.Turkce
            };
            context.AnahtarDegerler.Add(title_TR);


            AnahtarDeger title_EN = new AnahtarDeger()
            {
                Anahtar = Tanimlamalar.Title,
                Deger = "MNS",
                GorunenAd = "MNS",
                Dil = Dil.Ingilizce
            };
            context.AnahtarDegerler.Add(title_EN);


            AnahtarDeger titleSirketAdi = new AnahtarDeger()
            {
                Anahtar = Tanimlamalar.TitleSirketAdi,
                Deger = "MNS",
                GorunenAd = "MNS",
                Dil = Dil.Yok
            };
            context.AnahtarDegerler.Add(titleSirketAdi);

            AnahtarDeger description = new AnahtarDeger()
            {
                Anahtar = Tanimlamalar.Description,
                Deger = "MNS",
                GorunenAd = "MNS",
                Dil = Dil.Yok
            };
            context.AnahtarDegerler.Add(description);

            context.AnahtarDegerler.Add(projeAdi_TR);
            AnahtarDeger projeAdi_EN = new AnahtarDeger()
            {
                Anahtar = Tanimlamalar.ProjeAdi,
                Deger = "MNS",
                GorunenAd = "MNS",
                Dil = Dil.Ingilizce
            };
            context.AnahtarDegerler.Add(projeAdi_EN);

            //////Telefon
            AnahtarDeger telefon = new AnahtarDeger()
            {
                Anahtar = Tanimlamalar.TelefonNumarasi,
                Deger = "0555 555 55 55",
                GorunenAd = "Telefon Numarası",
                Dil = Dil.Yok
            };
            context.AnahtarDegerler.Add(telefon);

            //////Faks
            AnahtarDeger faks = new AnahtarDeger()
            {
                Anahtar = Tanimlamalar.Faks,
                Deger = "0555 555 55 55",
                GorunenAd = "Faks",
                Dil = Dil.Yok
            };
            context.AnahtarDegerler.Add(faks);

            //////Mail
            AnahtarDeger email = new AnahtarDeger()
            {
                Anahtar = Tanimlamalar.Email,
                Deger = "test@test.com",
                GorunenAd = "E-mail",
                Dil = Dil.Yok
            };
            context.AnahtarDegerler.Add(email);

            //////Adres
            AnahtarDeger adres_TR = new AnahtarDeger()
            {
                Anahtar = Tanimlamalar.Adres,
                Deger = "Adres",
                GorunenAd = "Adres",
                Dil = Dil.Turkce
            };
            context.AnahtarDegerler.Add(adres_TR);
            AnahtarDeger adres_EN = new AnahtarDeger()
            {
                Anahtar = Tanimlamalar.Adres,
                Deger = "Adres",
                GorunenAd = "Adres",
                Dil = Dil.Ingilizce
            };
            context.AnahtarDegerler.Add(adres_EN);

            //////Footer Yazı
            AnahtarDeger footerYazi_TR = new AnahtarDeger()
            {
                Anahtar = Tanimlamalar.FooterYazi,
                Deger = "Footer açıklama yazısı",
                GorunenAd = "Açıklama Yazısı",
                Dil = Dil.Turkce
            };
            context.AnahtarDegerler.Add(footerYazi_TR);
            AnahtarDeger footerYazi_EN = new AnahtarDeger()
            {
                Anahtar = Tanimlamalar.FooterYazi,
                Deger = "Footer description.",
                GorunenAd = "Açıklama Yazısı",
                Dil = Dil.Ingilizce
            };
            context.AnahtarDegerler.Add(footerYazi_EN);

            //////Hakkımızda Sayfası
            AnahtarDeger hakkimizda_TR = new AnahtarDeger()
            {
                Anahtar = Tanimlamalar.HakkimizdaSayfasi,
                Deger = "Hakkımızda  ",
                GorunenAd = "Hakkımızda Sayfası",
                Dil = Dil.Turkce
            };
            context.AnahtarDegerler.Add(hakkimizda_TR);
            AnahtarDeger hakkimizda_EN = new AnahtarDeger()
            {
                Anahtar = Tanimlamalar.HakkimizdaSayfasi,
                Deger = "Hakkımızda  ingilizce",
                GorunenAd = "Hakkımızda Sayfası",
                Dil = Dil.Ingilizce
            };
            context.AnahtarDegerler.Add(hakkimizda_EN);

            //////Sorular Sayfası
            AnahtarDeger sorular_TR = new AnahtarDeger()
            {
                Anahtar = Tanimlamalar.SorularSayfasi,
                Deger = "",
                GorunenAd = "Sorular Sayfası",
                Dil = Dil.Turkce
            };
            context.AnahtarDegerler.Add(sorular_TR);
            AnahtarDeger sorular_EN = new AnahtarDeger()
            {
                Anahtar = Tanimlamalar.SorularSayfasi,
                Deger = "",
                GorunenAd = "Sorular Sayfası",
                Dil = Dil.Ingilizce
            };
            context.AnahtarDegerler.Add(sorular_EN);

            //////Online Satis Sayfası
            AnahtarDeger onlinesatis_TR = new AnahtarDeger()
            {
                Anahtar = Tanimlamalar.OnlineSatisSayfasi,
                Deger = "",
                GorunenAd = "OnlineSatis Sayfası",
                Dil = Dil.Turkce
            };
          

            //////Anasayfa Banner
            AnahtarDeger anasayfaBanner_TR = new AnahtarDeger()
            {
                Anahtar = Tanimlamalar.AnaSayfaBanner,
                Deger = JsonConvert.SerializeObject(new AnaSayfaBanner()
                {
                    Link = "www.google.com",
                    Html = "Banner açıklama yazısı",
                    Resim = "/Client/assets/img/sessiz-ders-calisma-imkani.jpg"
                }),
                GorunenAd = "Ana Sayfa Banner",
                Dil = Dil.Turkce
            };
            context.AnahtarDegerler.Add(anasayfaBanner_TR);
            AnahtarDeger anasayfaBanner_EN = new AnahtarDeger()
            {
                Anahtar = Tanimlamalar.AnaSayfaBanner,
                Deger = JsonConvert.SerializeObject(new AnaSayfaBanner()
                {
                    Link = "www.google.com",
                    Html = "Banner açıklama yazısı",
                    Resim = "/Client/assets/img/sessiz-ders-calisma-imkani.jpg"

                }),
                GorunenAd = "Ana Sayfa Banner",
                Dil = Dil.Ingilizce
            };
            context.AnahtarDegerler.Add(anasayfaBanner_EN);

            //////İmkanlar Sayfası
            AnahtarDeger imkanlarSayfasi_TR = new AnahtarDeger()
            {
                Anahtar = Tanimlamalar.ImkanlarSayfasi,
                Deger = "<p></p>",
                GorunenAd = "İmkanlar Sayfası",
                Dil = Dil.Turkce
            };
            context.AnahtarDegerler.Add(imkanlarSayfasi_TR);
            AnahtarDeger imkanlarSayfasi_EN = new AnahtarDeger()
            {
                Anahtar = Tanimlamalar.ImkanlarSayfasi,
                Deger = "<p></p> ",
                GorunenAd = "İmkanlar Sayfası",
                Dil = Dil.Ingilizce
            };
            context.AnahtarDegerler.Add(imkanlarSayfasi_EN);

            List<UrunKategori> urunKategorileri = context.UrunKategorileri.Where(x => x.Dil == Dil.Turkce).ToList();
            //////ResimGalerisi
            AnahtarDeger resimGalerisi = new AnahtarDeger()
            {
                Anahtar = Tanimlamalar.ResimGalerisi,
                Deger = JsonConvert.SerializeObject(new List<ResimGalerisi>() {

                    new ResimGalerisi()
                    {
                        Id=Guid.NewGuid().ToString(),
                        Sira=1,
                        ResimYolu="/img/resim-galerisi/resim-yok1.png",
                        UrunKategoriId=urunKategorileri[0].Id,
                        KategoriAdi=urunKategorileri[0].Ad

                    },
                    new ResimGalerisi()
                    {
                        Id=Guid.NewGuid().ToString(),
                        Sira=2,
                        ResimYolu="/img/resim-galerisi/resim-yok1.png",
                        UrunKategoriId=urunKategorileri[0].Id,
                        KategoriAdi=urunKategorileri[0].Ad
                    },
                    new ResimGalerisi()
                    {
                        Id=Guid.NewGuid().ToString(),
                        Sira=3,
                        ResimYolu="/img/resim-galerisi/resim-yok1.png",
                        UrunKategoriId=urunKategorileri[1].Id,
                        KategoriAdi=urunKategorileri[1].Ad
                    },
                    new ResimGalerisi()
                    {
                        Id=Guid.NewGuid().ToString(),
                        Sira=4,
                        ResimYolu="/img/resim-galerisi/resim-yok1.png",
                        UrunKategoriId=urunKategorileri[1].Id,
                        KategoriAdi=urunKategorileri[1].Ad
                    },
                }),
                GorunenAd = "Resim Galerisi",
                Dil = Dil.Yok
            };
            context.AnahtarDegerler.Add(resimGalerisi);

            //////Yorumlar Sayfası
            AnahtarDeger yorumlarSayfasi_TR = new AnahtarDeger()
            {
                Anahtar = Tanimlamalar.YorumlarSayfasi,
                Deger = JsonConvert.SerializeObject(new List<Yorum>() {
                    new Yorum()
                    {
                        Id=Guid.NewGuid().ToString(),
                        AdSoyad="Yusuf ALBAYRAK",
                        Isi="Mobil Developer",
                        YorumIcerik="Bu yurtta, kalmaktan çok memnun kaldım."
                    }
                }),
                GorunenAd = "Yorumlar Sayfası",
                Dil = Dil.Turkce
            };
            context.AnahtarDegerler.Add(yorumlarSayfasi_TR);
            AnahtarDeger yorumlarSayfasi_EN = new AnahtarDeger()
            {
                Anahtar = Tanimlamalar.YorumlarSayfasi,
                Deger = JsonConvert.SerializeObject(new List<Yorum>() {
                    new Yorum()
                    {
                        Id=Guid.NewGuid().ToString(),
                        AdSoyad="Yusufe ALBAYRAK",
                        Isi="Mobil Developer",
                        YorumIcerik="In this dorm, I was very pleased to stay."
                    }
                }),
                GorunenAd = "Yorumlar Sayfası",
                Dil = Dil.Ingilizce
            };
            context.AnahtarDegerler.Add(yorumlarSayfasi_EN);

            //////GoogleMap
            AnahtarDeger googleMap = new AnahtarDeger()
            {
                Anahtar = Tanimlamalar.GoogleMap,
                Deger = "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3022.931719764981!2d30.336672714905355!3d40.74152794374634!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x409df62920b164e5%3A0xd78de65fbe0ab935!2sArgede%20Bili%C5%9Fim%20Teknolojileri!5e0!3m2!1str!2str!4v1619767381817!5m2!1str!2str",
                GorunenAd = "Google Map",
                Dil = Dil.Yok
            };
            context.AnahtarDegerler.Add(googleMap);

          

            //////MainKeywords
            AnahtarDeger mainkeywords = new AnahtarDeger()
            {
                Anahtar = Tanimlamalar.MainKeywords,
                Deger = "MNS",
                GorunenAd = "MainKeywords",
                Dil = Dil.Yok
            };
            context.AnahtarDegerler.Add(mainkeywords);


            //////Site Adı
            AnahtarDeger siteAdi = new AnahtarDeger()
            {
                Anahtar = Tanimlamalar.SiteAdi,
                Deger = "https://www.google.com",
                GorunenAd = "Site",
                Dil = Dil.Yok
            };
            context.AnahtarDegerler.Add(siteAdi);

            //////Instagram Adresi
            AnahtarDeger instagram = new AnahtarDeger()
            {
                Anahtar = Tanimlamalar.Instagram,
                Deger = "https://www.instagram.com",
                GorunenAd = "Instagram",
                Dil = Dil.Yok
            };
            context.AnahtarDegerler.Add(instagram);

            //////Twitter Adresi
            AnahtarDeger twitter = new AnahtarDeger()
            {
                Anahtar = Tanimlamalar.Twitter,
                Deger = "https://www.twitter.com",
                GorunenAd = "Twitter",
                Dil = Dil.Yok
            };
            context.AnahtarDegerler.Add(twitter);

            //////Facebook Adresi
            AnahtarDeger facebook = new AnahtarDeger()
            {
                Anahtar = Tanimlamalar.Facebook,
                Deger = "https://www.facebook.com",
                GorunenAd = "Facebook",
                Dil = Dil.Yok
            };
            context.AnahtarDegerler.Add(facebook);


        

            context.SaveChanges();
        }
    }
}