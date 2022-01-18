using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Interfaces.BusinessLogicLayers;
using ArgedeSP.Contracts.Interfaces.Repositories;
using ArgedeSP.Contracts.Models.Common;
using ArgedeSP.Contracts.Models.DTO.Kulanici.Req;
using ArgedeSP.Contracts.Models.DTO.Kulanici.Res;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.BLL.BusinessServices
{
    public class KullaniciBS : IKullaniciBS
    {
        private IKullaniciRepository _kullaniciRepository;

        private UserManager<Kullanici> _userManager;
        private RoleManager<Rol> _roleManager;
        private IPasswordHasher<Kullanici> _passwordHasher;

        public KullaniciBS(
            IKullaniciRepository kullaniciRepository,

            UserManager<Kullanici> userManager,
            RoleManager<Rol> roleManager,
            IPasswordHasher<Kullanici> _passwordHasher)
        {
            _kullaniciRepository = kullaniciRepository;

            _userManager = userManager;
            _roleManager = roleManager;

        }

        public async Task<OperationResult> KullaniciBilgileriGetir(string userName)
        {
            return OperationResult.Success(await _kullaniciRepository.GetAllIncluding()
                  .FirstOrDefaultAsync(x => x.UserName == userName));
        }

        public async Task<OperationResult> KullaniciEkle(Kullanici_REQ kullaniciEkle_REQ)
        {
            try
            {
                Kullanici hesap = new Kullanici();

                Kullanici kullaniciEmail_KONTROL = await _kullaniciRepository.FindAsync(x => x.Email == kullaniciEkle_REQ.Email);
                if (kullaniciEmail_KONTROL != null)
                {
                    return OperationResult.Error(MesajKodu.KullaniciEmailiZatenVar);
                }


                if (string.IsNullOrWhiteSpace(kullaniciEkle_REQ.Password))
                {
                    return OperationResult.Error(MesajKodu.SifreZorunlu);
                }


                hesap.UserName = kullaniciEkle_REQ.Email;
                hesap.Email = kullaniciEkle_REQ.Email;
                hesap.SecurityStamp = Guid.NewGuid().ToString();
                hesap.ProfilResmi = kullaniciEkle_REQ.Resim;
                hesap.PhoneNumber = kullaniciEkle_REQ.PhoneNumber;
                hesap.Ad = kullaniciEkle_REQ.Ad;
                hesap.Soyad = kullaniciEkle_REQ.Soyad;
                hesap.Durum = kullaniciEkle_REQ.Durum ? Durum.Aktif : Durum.Pasif;
                hesap.TCKN = kullaniciEkle_REQ.TCKN;
                hesap.OlusturmaTarihi = DateTime.Now;


                var clientResult = (await _userManager.CreateAsync(hesap, kullaniciEkle_REQ.Password));
                if (!clientResult.Succeeded)
                {
                    return OperationResult.Error(MesajKodu.KullaniciEklenemedi);
                }

                var roleResult = await _userManager.AddToRoleAsync(hesap, "Ogrenci");
                if (!roleResult.Succeeded)
                {
                    return OperationResult.Error(MesajKodu.RolEklenemedi);
                }

                return OperationResult.Success(new { KullaniciId = hesap.Id });
            }
            catch (Exception ex)
            {
                Log.Error(ex, nameof(KullaniciEkle) + " {@params}", kullaniciEkle_REQ);
                return OperationResult.Error(MesajKodu.BeklenmedikHata);
            }
        }

        public async Task<OperationResult> KullaniciGetirIdIle(string kullaniciId)
        {
            return OperationResult.Success(await _kullaniciRepository.FindAsync(x => x.Id == kullaniciId));
        }

        public async Task<OperationResult> KullaniciGuncelle(Kullanici_REQ kullaniciGuncelle_REQ)
        {
            try
            {
                Kullanici hesap = await _kullaniciRepository.FindAsync(x => x.Id == kullaniciGuncelle_REQ.Id);
                if (hesap == null)
                {
                    return OperationResult.Error("Kullanıcı Bulunamadı");
                }

                Kullanici kullaniciEmail_KONTROL = await _kullaniciRepository.FindAsync(x => x.Email == kullaniciGuncelle_REQ.Email && x.Email != kullaniciGuncelle_REQ.Email);
                if (kullaniciEmail_KONTROL != null)
                {
                    return OperationResult.Error(MesajKodu.KullaniciEmailiZatenVar);
                }



                hesap.UserName = kullaniciGuncelle_REQ.Email;
                hesap.Email = kullaniciGuncelle_REQ.Email;

                hesap.ProfilResmi = kullaniciGuncelle_REQ.Resim;
                hesap.PhoneNumber = kullaniciGuncelle_REQ.PhoneNumber;
                hesap.Ad = kullaniciGuncelle_REQ.Ad;
                hesap.Soyad = kullaniciGuncelle_REQ.Soyad;
                hesap.Durum = kullaniciGuncelle_REQ.Durum ? Durum.Aktif : Durum.Pasif;
                hesap.TCKN = kullaniciGuncelle_REQ.TCKN;

                await _userManager.UpdateAsync(hesap);

                if (!string.IsNullOrWhiteSpace(kullaniciGuncelle_REQ.Password))
                {
                    await _userManager.RemovePasswordAsync(hesap);
                    await _userManager.AddPasswordAsync(hesap, kullaniciGuncelle_REQ.Password);
                }

                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                Log.Error(ex, nameof(KullaniciGuncelle) + " {@params}", kullaniciGuncelle_REQ);
                return OperationResult.Error(MesajKodu.BeklenmedikHata);
            }
        }

        public async Task<OperationResult> KullaniciGuncelleClient(string kimlik, string sifre, bool ProfilGizliMi)
        {
            try
            {
                Kullanici hesap = await _kullaniciRepository.FindAsync(x => x.Id == kimlik);
                if (hesap == null)
                {
                    return OperationResult.Error("Kullanıcı Bulunamadı");
                }
                hesap.PasswordHash = sifre;

                await _userManager.UpdateAsync(hesap);

                if (!string.IsNullOrWhiteSpace(sifre))
                {
                    await _userManager.RemovePasswordAsync(hesap);
                    await _userManager.AddPasswordAsync(hesap, sifre);
                }

                return OperationResult.Success();
            }
            catch (Exception ex)
            {
               
                return OperationResult.Error(MesajKodu.BeklenmedikHata);
            }
        }

        public OperationResult KullanicilariListele(KullaniciListele_REQ kullaniciListele_REQ)
        {
            try
            {
                return _kullaniciRepository.SayfalaAramaIle(kullaniciListele_REQ);
            }
            catch (Exception ex)
            {
                Log.Error(ex, nameof(KullanicilariListele) + " {@params}", kullaniciListele_REQ);
                return OperationResult.Error(MesajKodu.BeklenmedikHata);
            }

        }


    }
}