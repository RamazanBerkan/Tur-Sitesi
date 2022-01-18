using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Helpers.Mail;
using ArgedeSP.Contracts.Interfaces.BusinessLogicLayers;
using ArgedeSP.Contracts.Models.Common;
using ArgedeSP.Contracts.Models.DTO.Hesap.Res;
using ArgedeSP.WebUI.Areas.Admin.Models.Hesap.Req;
using ArgedeSP.WebUI.Areas.Admin.Models.Hesap.Res;
using ArgedeSP.WebUI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HesapController : Controller
    {

        private UserManager<Kullanici> _userManager;
        private SignInManager<Kullanici> _signInManager;
        private RoleManager<Rol> _roleManager;
        private readonly IEmailSender2 _emailSender;
        private readonly IRazorPartialToStringRenderer _renderer;
        private IAnahtarDegerBS _anahtarDegerBS;

        public HesapController(IPasswordHasher<Kullanici> passwordHasher,
            UserManager<Kullanici> userManager,
            SignInManager<Kullanici> signInManager,
            RoleManager<Rol> roleManager,
            IEmailSender2 emailSender,
           IRazorPartialToStringRenderer renderer,
           IAnahtarDegerBS anahtarDegerBS)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _renderer = renderer;
            _anahtarDegerBS = anahtarDegerBS;
        }



        [HttpGet]
        [AllowAnonymous]
        public IActionResult Giris(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                    return Redirect(returnUrl ?? "/");
                if (User.IsInRole("Ogrenci"))
                {
                    if (!string.IsNullOrWhiteSpace(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("AnaSayfa", "AnaSayfa", new { area = "Ogrenci" });
                    }
                }
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(new Giris_Req());

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Giris(Giris_Req girisModel, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                    return Redirect(returnUrl ?? "/");
                if (User.IsInRole("Ogrenci"))
                {
                    if (!string.IsNullOrWhiteSpace(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("AnaSayfa", "AnaSayfa", new { area = "Ogrenci" });
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                return View(girisModel);

            }

            var user = await _userManager.FindByNameAsync(girisModel.KullaniciAdi);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(girisModel.KullaniciAdi);
                if (user == null)
                {
                    ModelState.AddModelError(nameof(girisModel.KullaniciAdi), "Bu kullanici bulunamadi");
                    return View(girisModel);
                }
            }

            if (user.Durum == Durum.Pasif)
            {
                ModelState.AddModelError(nameof(girisModel.KullaniciAdi), "Bu hesap engellenmiştir Lütfen yönetici ile iletişime geçiniz");
                return View(girisModel);
            }

            await _signInManager.SignOutAsync();
            var result = await _signInManager.PasswordSignInAsync(user, girisModel.Sifre, false, false);


            if (result.Succeeded)
            {
                IList<string> userRole = await _userManager.GetRolesAsync(user);
                if (userRole.Contains("Admin"))
                    return RedirectToAction("AnaSayfa", "AnaSayfa", new { area = "Admin" });

                if (userRole.Contains("Ogrenci"))
                    return RedirectToAction("AnaSayfa", "AnaSayfa", new { area = "Ogrenci" });

                return Redirect(returnUrl ?? "/");
            }

            ModelState.AddModelError(nameof(girisModel.Sifre), "Kullanıcı adı veya şifre hatalı");

            return View(girisModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> GirisAjax([FromBody]Giris_Req girisModel, string returnUrl)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return Json(new
                {
                    basariliMi = true,
                    donusUrl = returnUrl ?? "/"
                });
            }

            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    basariliMi = false,
                    hatalar = Helpers.Helpers.GetErrorListFromModelState(ModelState)
                });

            }

            var user = await _userManager.FindByNameAsync(girisModel.KullaniciAdi);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(girisModel.KullaniciAdi);
                if (user == null)
                {
                    return Json(new
                    {
                        basariliMi = false,
                        hatalar = new List<string>()
                    {
                        "Kullanıcı adi bulunamadı"
                    }
                    });
                }
            }
            if (user.Durum == Durum.Pasif)
            {
                return Json(new
                {
                    basariliMi = false,
                    hatalar = new List<string>()
                    {
                        "Bu hesap engellenmiştir Lütfen yönetici ile iletişime geçiniz"
                    }
                });
            }
            await _signInManager.SignOutAsync();
            var result = await _signInManager.PasswordSignInAsync(user, girisModel.Sifre, false, false);

            if (result.Succeeded)
            {
                IList<string> userRole = await _userManager.GetRolesAsync(user);
                if (userRole.Contains("Admin"))
                {
                    return Json(new
                    {
                        basariliMi = true,
                        donusUrl = Url.Action("AnaSayfa", "AnaSayfa", new { area = "Admin" })
                    });
                }

                return Json(new
                {
                    basariliMi = true,
                    donusUrl = returnUrl
                });

            }

            ModelState.AddModelError(nameof(girisModel.Sifre), "");
            return Json(new
            {
                basariliMi = false,
                hatalar = new List<string>()
                    {
                        "Kullanıcı adı veya şifre hatalı"
                    }
            });
        }

        [HttpGet]
        public async Task<IActionResult> Cikis()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SifremiUnuttum(string email)
        {
            if (string.IsNullOrEmpty(email))
                return View();

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
               
                TempData["Hata"] = "Email adresi bulunamadı.";
                return RedirectToAction("Giris");
            }

            var confrimationCode =
                    await _userManager.GeneratePasswordResetTokenAsync(user);


            AnahtarDeger projeAdi = (AnahtarDeger)_anahtarDegerBS.AnahtarGetir(Dil.Turkce, Tanimlamalar.ProjeAdi).ReturnObject;
            AnahtarDeger webSitesi = (AnahtarDeger)_anahtarDegerBS.AnahtarGetir(Dil.Yok, Tanimlamalar.SiteAdi).ReturnObject;


            SifremiUnuttum_Res sifremiUnuttum_Res = new SifremiUnuttum_Res()
            {
                KullaniciId = user.Id,
                DogrulamaKodu = confrimationCode,
                AdSoyad = user.AdSoyad(),
                ProjeAdi = projeAdi.Deger,
                WebSitesi = webSitesi.Deger
            };

            var emailHtml = await _renderer.RenderPartialToStringAsync("~/Areas/Admin/Views/Hesap/_SifremiUnuttum_EMAIL.cshtml", sifremiUnuttum_Res);
            await _emailSender.SendEmailAsync(
                email: user.Email,
                subject: "Şifre Sıfırlama",
                htmlMessage: emailHtml,
                projectName: projeAdi.Deger);

            TempData["Basarili"] = "E-mail adresine, şifre sıfırlama e-postası başarıyla gönderildi.";
            return RedirectToAction("Giris");
        }

        [AllowAnonymous]
        public IActionResult ResetSifre(string kullaniciId, string dogrulamaKodu)
        {
            if (kullaniciId == null || dogrulamaKodu == null)
                throw new ApplicationException("Parametreler yanlış");

            var model = new ResetSifre_Req { DogrulamaKodu = dogrulamaKodu };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetSifre(ResetSifre_Req model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "Email hatalı");
                return View(model);
            }

            var result = await _userManager.ResetPasswordAsync(
                                        user, model.DogrulamaKodu, model.Sifre);
            if (result.Succeeded)
            {
                TempData["Basarili"] = "Şifreniz başarıyla sıfırlandı.";
                return RedirectToAction("Giris");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("BeklenmedikHata", error.Description);

            return View(model);
        }

    }

}