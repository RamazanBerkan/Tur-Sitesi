using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Models.Common;
using ArgedeSP.WebUI.Helpers;
using ArgedeSP.WebUI.Helpers.Extantions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;

namespace ArgedeSP.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DosyaIslemleriController : Controller
    {
        private UserManager<Kullanici> _userManager;
        private SignInManager<Kullanici> _signInManager;
        private RoleManager<Rol> _roleManager;
        private readonly IRazorPartialToStringRenderer _renderer;
        private readonly IFileProvider _fileProvider;

        public DosyaIslemleriController(
                        UserManager<Kullanici> userManager,
                        SignInManager<Kullanici> signInManager,
                        RoleManager<Rol> roleManager,
                         IRazorPartialToStringRenderer renderer,
                        IFileProvider fileProvider)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _renderer = renderer;
            _fileProvider = fileProvider;
        }


        public IActionResult Index(string dosyaAdi, int sayfa = 1, int sayfaBoyutu = 10)
        {
            List<IFileInfo> resimler = null;

            if (!string.IsNullOrWhiteSpace(dosyaAdi))
            {
                resimler = _fileProvider.GetDirectoryContents("wwwroot/img/" + dosyaAdi).ToList();
            }


            List<DosyaYukleme_RES> medias = resimler?.Select(x => new DosyaYukleme_RES()
            {
                DosyaAdi = x.Name,
                DosyaBoyutu = x.Length,
                DosyaUrl = UrlExtansions.GetUrlFromAbsolutePath(x.PhysicalPath)
            }).ToList();

            return View(medias ?? new List<DosyaYukleme_RES>());
        }

        [HttpDelete]
        public IActionResult DeleteFile(string fileUrl)
        {
            if (!string.IsNullOrEmpty(fileUrl))
            {

                var deletePath = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot/" + fileUrl);

                FileInfo file = new FileInfo(deletePath);

                if (file.Exists)
                {
                    file.Delete();

                    return Json(new Genel_RES()
                    {
                        BasariliMi = true,
                        Mesaj = "Dosya başarıyla silindi.",
                        Model = null,
                    });
                }
            }

            return Json(new Genel_RES()
            {
                BasariliMi = false,
                Mesaj = "Dosya silinemedi",
                Model = null,
            });
        }
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, string dosyaAdi)
        {
            DosyaYukleme_RES media = null;
            if (file != null && file.Length > 0)
            {

                if (file.ContentType != "image/jpeg" && file.ContentType != "image/jpg" && file.ContentType != "image/png" && file.ContentType != "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && file.ContentType != "application/pdf" && file.ContentType != "application/x-zip-compressed")
                {
                    return BadRequest("Desteklenmeyen dosya uzantısı");
                }

                var uploads = Path.Combine(
            Directory.GetCurrentDirectory(), "wwwroot/" + dosyaAdi);

                bool controll = true;
                Random random = new Random();
                string newFileName = file.FileName;

                do
                {
                    FileInfo exists = new FileInfo(uploads + "/" + newFileName);
                    if (exists.Exists)
                    {
                        controll = true;
                        newFileName = random.Next(1, 1000).ToString() + "-" + file.FileName;
                    }
                    else
                    {
                        controll = false;
                        var newPath = Path.Combine(uploads, newFileName);
                        using (var fileStream = new FileStream(newPath, FileMode.Create))
                        {
                            double size = (file.Length / 1024f) / 1024f;
                            media = new DosyaYukleme_RES()
                            {
                                BasariliMi = true,
                                DosyaAdi = file.FileName,
                                DosyaBoyutu = Math.Round(size, 2),
                                DosyaUrl = UrlExtansions.GetUrlFromAbsolutePath(newPath)
                            };
                            await file.CopyToAsync(fileStream);
                        }
                    }

                } while (controll);

            }


            return Json(media);
        }
        [HttpPost]
        public async Task<IActionResult> CFUpload(IFormFile upload, string dosyaAdi)
        {
            DosyaYukleme_RES media = null;
            if (upload != null && upload.Length > 0)
            {

                if (upload.ContentType != "image/jpeg" && upload.ContentType != "image/jpg" && upload.ContentType != "image/png" && upload.ContentType != "application/docx" && upload.ContentType != "application/pdf" && upload.ContentType != "zip")
                {
                    var LengthErrorMessage = "Desteklenmeyen dosya uzantısı";
                    dynamic stuff = JsonConvert.DeserializeObject("{ 'uploaded': 0, 'error': { 'message': \"" + LengthErrorMessage + "\"}}");
                    return Json(stuff);
                }

                var uploads = Path.Combine(
            Directory.GetCurrentDirectory(), "wwwroot/" + dosyaAdi);

                bool controll = true;
                Random random = new Random();
                string newFileName = upload.FileName;

                do
                {
                    FileInfo exists = new FileInfo(uploads + "/" + newFileName);
                    if (exists.Exists)
                    {
                        controll = true;
                        newFileName = random.Next(1, 1000).ToString() + "-" + upload.FileName;
                    }
                    else
                    {
                        controll = false;
                        var newPath = Path.Combine(uploads, newFileName);
                        using (var fileStream = new FileStream(newPath, FileMode.Create))
                        {
                            double size = (upload.Length / 1024f) / 1024f;
                            media = new DosyaYukleme_RES()
                            {
                                BasariliMi = true,
                                DosyaAdi = newFileName,
                                DosyaBoyutu = Math.Round(size, 2),
                                DosyaUrl = UrlExtansions.GetUrlFromAbsolutePath(newPath)
                            };
                            await upload.CopyToAsync(fileStream);
                        }
                    }

                } while (controll);

            }
            else
            {
                var LengthErrorMessage = "Dosya yüklenemedi";
                dynamic stuff = JsonConvert.DeserializeObject("{ 'uploaded': 0, 'error': { 'message': \"" + LengthErrorMessage + "\"}}");
                return Json(stuff);
            }

            var successMessage = "Resim sunucuya başarı ile yüklendi";
            dynamic success = JsonConvert.DeserializeObject("{ 'uploaded': 1,'fileName': \"" + media.DosyaAdi + "\",'url': \"" + media.DosyaUrl + "\", 'error': { 'message': \"" + successMessage + "\"}}");
            return Json(success);
        }
        [HttpPost]
        public JsonResult ResimleriGetir(string dosyaAdi)
        {
            IDirectoryContents makaleResimleri = null;
            List<IFileInfo> files = null;

            makaleResimleri = _fileProvider.GetDirectoryContents("wwwroot/" + dosyaAdi);
            files = makaleResimleri.ToList();

            List<string> medias = files?.OrderByDescending(x => x.LastModified).Select(x => UrlExtansions.GetUrlFromAbsolutePath(x.PhysicalPath)).ToList();
            return Json(medias);

        }
    }
}