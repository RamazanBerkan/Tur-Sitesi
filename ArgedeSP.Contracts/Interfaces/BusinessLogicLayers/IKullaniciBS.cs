using ArgedeSP.Contracts.Models.Common;
using ArgedeSP.Contracts.Models.DTO.Kulanici.Req;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ArgedeSP.Contracts.Interfaces.BusinessLogicLayers
{
    public interface IKullaniciBS
    {
      
        OperationResult KullanicilariListele(KullaniciListele_REQ kullaniciListele_REQ);
        Task<OperationResult> KullaniciEkle(Kullanici_REQ kullaniciEkle_REQ);
        Task<OperationResult> KullaniciGuncelle(Kullanici_REQ kullaniciEkle_REQ);
        Task<OperationResult> KullaniciBilgileriGetir(string userName);
        Task<OperationResult> KullaniciGetirIdIle(string kullaniciId);
        Task<OperationResult> KullaniciGuncelleClient(string kimlik, string sifre, bool ProfilGizliMi);
    }
}
