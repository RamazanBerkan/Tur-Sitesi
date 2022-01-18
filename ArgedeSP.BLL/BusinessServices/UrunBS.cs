using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Helpers.Extantions;
using ArgedeSP.Contracts.Interfaces.BusinessLogicLayers;
using ArgedeSP.Contracts.Interfaces.Repositories;
using ArgedeSP.Contracts.Models.Common;
using ArgedeSP.Contracts.Models.DTO.Urunler.Req;
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
    public class UrunBS : IUrunBS
    {
        private IUrunRepository _urunRepository;
        private IUrunKategoriRepository _urunKategoriRepository;

        public UrunBS(
            IUrunRepository urunRepository,
            IUrunKategoriRepository urunKategoriRepository
            )
        {
            _urunKategoriRepository = urunKategoriRepository;
            _urunRepository = urunRepository;
        }

        public OperationResult AktifUrunleriGetir(Dil SuankiDil, string altKategoriAdi, string filtre)
        {
            try
            {
                IQueryable<Urun> urunler = _urunRepository.GetAllIncluding(x => x.UrunKategori.UstKategori).Include(x => x.UrunResimGalerisi).Where(x => x.Durum == Enums.Durum.Aktif).Where(x => x.Dil == SuankiDil);

                if (!string.IsNullOrWhiteSpace(altKategoriAdi))
                    urunler = urunler.Where(x => x.UrunKategori.SeoUrl == altKategoriAdi);

                if (!string.IsNullOrWhiteSpace(filtre))
                    urunler = urunler.Where(x => x.UrunAdi.Trim().Replace(" ", "").ToUpper().Contains(filtre.Trim().Replace(" ", "").ToUpper()));
                urunler.OrderBy(x => x.UrunSira).ToList();
                return OperationResult.Success(urunler.ToList());
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{nameof(AktifUrunleriGetir)} fonksiyonunda hata");
                return OperationResult.Error(Enums.MesajKodu.BeklenmedikHata);
            }

        }

        public OperationResult AktifUrunleriGetirAnsayfa(Dil SuankiDil)
        {
            try
            {
                IList<Urun> urunler = _urunRepository.GetAllIncluding(x => x.UrunKategori.UstKategori, x => x.UrunResimGalerisi).Where(x => x.Durum == Enums.Durum.Aktif).Where(x => x.Dil == SuankiDil).Take(6).ToList();
                return OperationResult.Success(urunler);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{nameof(AktifUrunleriGetir)} fonksiyonunda hata");
                return OperationResult.Error(Enums.MesajKodu.BeklenmedikHata);
            }

        }
        public OperationResult RandomUrunGetir(int adet)
        {
            try
            {
                Random rnd = new Random();
                IList<Urun> urunler = _urunRepository.GetAllIncluding().Where(x => x.Durum == Enums.Durum.Aktif).Skip(rnd.Next(0, _urunRepository.Count())).Take(adet).ToList();
                return OperationResult.Success(urunler);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{nameof(RandomUrunGetir)} fonksiyonunda hata");
                return OperationResult.Error(Enums.MesajKodu.BeklenmedikHata);
            }
        }


        public OperationResult RandomKategoriGetir(int adet, Dil SuankiDil,  string altKategoriAdi )
        {
            try
            {
                Random rnd = new Random();
                IQueryable<Urun> urunler = _urunRepository.GetAllIncluding(x => x.UrunKategori.UstKategori).Where(x => x.Durum == Enums.Durum.Aktif).Where(x => x.Dil == SuankiDil);

                if (!string.IsNullOrWhiteSpace(altKategoriAdi))
                    urunler = urunler.Where(x => x.UrunKategori.SeoUrl == altKategoriAdi);

                urunler = urunler.Skip(rnd.Next(0, urunler.Count())).Take(adet);
                urunler.OrderBy(x => x.UrunSira).ToList();

                var _urunler = urunler.ToList();
                return OperationResult.Success(urunler.ToList());
               // return OperationResult.Success(_urunler.Where(x => _urunler.IndexOf( x) == rnd.Next(0, urunler.Count()));

                //int sira = 0, i = 0;
                //while(sira <  urunler.Count() && i<adet)
                //{
                //   sira = rnd.Next(0, urunler.Count()) ;                 
                //    Urun urun1 = _urunler[sira];
                   
                //    i++;
                //}

            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{nameof(AktifUrunleriGetir)} fonksiyonunda hata");
                return OperationResult.Error(Enums.MesajKodu.BeklenmedikHata);
            }
        }

        public async Task<OperationResult> UrunEkle(UrunEkle_REQ inputEt)
        {
            try
            {
                UrunKategori urunKategori = _urunKategoriRepository.Find(x => x.Id == inputEt.UrunKategoriId);
                if (urunKategori == null)
                {
                    return OperationResult.Error(MesajKodu.UrunKategoriBulunamadı);
                }

                Urun urunSeo_KONTROL = _urunRepository.Find(x => x.SeoUrl == inputEt.SeoUrl.FriendlyUrl() && x.Id != inputEt.Id);
                if (urunSeo_KONTROL != null)
                {
                    return OperationResult.Error(MesajKodu.SeoUrlZatenVar);
                }

                Urun urun = new Urun()
                {
                    Durum = inputEt.Durum ? Durum.Aktif : Durum.Pasif,
                    KisaAciklama = inputEt.KisaAciklama,
                    OlusturmaTarihi = DateTime.Now,
                    Resim = inputEt.AnaResim,
                    UrunAdi = inputEt.UrunAdi,
                    Keywords = inputEt.Keywords,
                    UrunSira = inputEt.UrunSira,
                    DetayAciklama = inputEt.DetayAciklama,
                    //UrunModel = inputEt.UrunModel,
                    UrunKategori = urunKategori,
                    UzunAciklama = inputEt.UzunAciklama,
                    TeknikOzellikler = inputEt.TeknikOzellikler,
                    NasilCalisir = inputEt.NasilCalisir,
                    /*
                                        UrunDokumanlari = inputEt.Dokumanlar.Select(x => new UrunDokumani()
                                        {
                                            OlusturmaTarihi = DateTime.Now,
                                            DokumanUrl = x,
                                        }).ToList(),
                                        */

                    UrunDokumanlari = inputEt.UrunDokumanlari.Select(x => new UrunDokumani()
                    {
                        OlusturmaTarihi = DateTime.Now,
                        DokumanUrl = x.DokumanUrl,
                        DokumanAdi = x.DokumanAdi,
                    }).ToList(),
                    UrunResimGalerisi = inputEt.ResimGalerisi.Select(x => new UrunResimGaleri()
                    {
                        OlusturmaTarihi = DateTime.Now,
                        ResimUrl = x,
                    }).ToList(),
                    SeoUrl = inputEt.SeoUrl.FriendlyUrl(),
                Fiyat=inputEt.Fiyat,
                IndirimliFiyat=inputEt.IndirimliFiyat,
                Indirim=inputEt.Indirim,

                    Dil = inputEt.Dil
                };

                await _urunRepository.InsertAsync(urun, true);

                return OperationResult.Success(urun);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{nameof(UrunEkle)} fonksiyonunda hata");
                return OperationResult.Error(MesajKodu.BeklenmedikHata);
            }


        }
        public async Task<OperationResult> UrunGetir(string seoUrl)
        {
            try
            {
                Urun urun = await _urunRepository
                    .GetAllIncluding(x=>x.UrunKategori.UstKategori.UstKategori).Select(x => new Urun()
                    {
                        Durum = x.Durum,
                        Id = x.Id,
                        KisaAciklama = x.KisaAciklama,
                        UzunAciklama = x.UzunAciklama,
                        TeknikOzellikler = x.TeknikOzellikler,
                        NasilCalisir = x.NasilCalisir,
                        OlusturmaTarihi = x.OlusturmaTarihi,
                        Resim = x.Resim,
                        SeoUrl = x.SeoUrl,
                        UrunAdi = x.UrunAdi,
                        Keywords = x.Keywords,
                        DetayAciklama = x.DetayAciklama,
                        //UrunModel = x.UrunModel,
                        UrunSira = x.UrunSira,
                        UrunKategori = x.UrunKategori,                        
                        UrunKategoriId = x.UrunKategoriId,
                        UrunDokumanlari = x.UrunDokumanlari.ToList(),
                        UrunResimGalerisi = x.UrunResimGalerisi.ToList(),
                        Fiyat = x.Fiyat,
                        IndirimliFiyat = x.IndirimliFiyat,
                        Indirim = x.Indirim,
                        Dil = x.Dil
                    })
                    .FirstOrDefaultAsync(x => x.SeoUrl == seoUrl);
                if (urun == null)
                {
                    return OperationResult.Error(MesajKodu.UrunBulunamadi);

                }
                return OperationResult.Success(urun);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{nameof(UrunGetir)} fonksiyonunda hata");
                return OperationResult.Error(MesajKodu.BeklenmedikHata);
            }
        }
        public async Task<OperationResult> UrunGetir(int urunId)
        {
            try
            {
                Urun urun = await _urunRepository.GetAllIncluding(x => x.UrunKategori, x => x.UrunResimGalerisi).FirstOrDefaultAsync(x => x.Id == urunId);
                if (urun == null)
                {
                    return OperationResult.Error(MesajKodu.UrunBulunamadi);

                }
                return OperationResult.Success(urun);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{nameof(UrunGetir)} fonksiyonunda hata");
                return OperationResult.Error(MesajKodu.BeklenmedikHata);
            }
        }
        public async Task<OperationResult> UrunGetirSorgusuz(int urunId)
        {
            try
            {
                Urun urun = await _urunRepository.GetAllIncluding(x => x.UrunKategori, x => x.UrunResimGalerisi, x => x.UrunDokumanlari).FirstOrDefaultAsync(x => x.Id == urunId);
                if (urun == null)
                {
                    return OperationResult.Error(MesajKodu.UrunBulunamadi);

                }
                return OperationResult.Success(urun);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{nameof(UrunGetir)} fonksiyonunda hata");
                return OperationResult.Error(MesajKodu.BeklenmedikHata);
            }
        }
        public async Task<OperationResult> UrunGuncelle(UrunEkle_REQ inputEt)
        {
            try
            {
                Urun urun = await _urunRepository.GetByIdAsync(inputEt.Id);
                if (urun == null)
                {
                    return OperationResult.Error(MesajKodu.UrunBulunamadi);

                }

                UrunKategori urunKategori = _urunKategoriRepository.Find(x => x.Id == inputEt.UrunKategoriId);
                if (urunKategori == null)
                {
                    return OperationResult.Error(MesajKodu.UrunKategoriBulunamadı);
                }

                Urun urunSeo_KONTROL = _urunRepository.Find(x => x.SeoUrl == inputEt.SeoUrl.FriendlyUrl() && x.Id != inputEt.Id);
                if (urunSeo_KONTROL != null)
                {
                    return OperationResult.Error(MesajKodu.SeoUrlZatenVar);
                }

                urun.KisaAciklama = inputEt.KisaAciklama;
                urun.Durum = inputEt.Durum ? Durum.Aktif : Durum.Pasif;
                urun.Resim = inputEt.AnaResim;
                urun.SeoUrl = inputEt.SeoUrl.FriendlyUrl();
                urun.UrunAdi = inputEt.UrunAdi;
                urun.Keywords = inputEt.Keywords;
                urun.UrunSira = inputEt.UrunSira;
                urun.DetayAciklama = inputEt.DetayAciklama;
                //urun.UrunModel = inputEt.UrunModel;
                urun.UrunKategori = urunKategori;
                urun.UrunKodu = inputEt.UrunKodu;
                urun.UzunAciklama = inputEt.UzunAciklama;
                urun.TeknikOzellikler = inputEt.TeknikOzellikler;
                urun.NasilCalisir = inputEt.NasilCalisir;
                /* urun.UrunDokumanlari = inputEt.Dokumanlar.Select(x => new UrunDokumani
                 {
                     DokumanAdi = inputEt.UrunAdi,
                     DokumanUrl = x,
                 }).ToList();*/
                urun.UrunDokumanlari = inputEt.UrunDokumanlari.Select(x => new UrunDokumani
                {
                    DokumanAdi = x.DokumanAdi,
                    DokumanUrl = x.DokumanUrl,
                }).ToList();
                urun.UrunResimGalerisi = inputEt.ResimGalerisi.Select(x => new UrunResimGaleri()
                {

                    OlusturmaTarihi = DateTime.Now,
                    ResimUrl = x,
                }).ToList();
                urun.Fiyat = inputEt.Fiyat;
                urun.IndirimliFiyat = inputEt.IndirimliFiyat;
                urun.Indirim = inputEt.Indirim;                
                urun.Dil = inputEt.Dil;

                _urunRepository.Commit();

                return OperationResult.Success(urun);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{nameof(UrunGetir)} fonksiyonunda hata");
                return OperationResult.Error(MesajKodu.BeklenmedikHata);
            }
        }
        public OperationResult UrunleriSayfala(int sayfa, int sayfaBoyutu, int id, string urunAdi, string urunKategori, Durum durum)
        {
            return OperationResult.Success(_urunRepository.SayfalaAramaIle(sayfa, sayfaBoyutu, id, urunAdi, urunKategori, durum));
        }
        public async Task<OperationResult> UrunSil(int id)
        {
            try
            {
                Urun urun = _urunRepository.GetAllIncluding().FirstOrDefault(x => x.Id == id);

                if (urun == null)
                {
                    return OperationResult.Error(MesajKodu.UrunBulunamadi);
                }


                await _urunRepository.DeleteAsync(urun.Id, true);
                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{nameof(UrunSil)} fonksiyonunda hata");
                return OperationResult.Error(MesajKodu.BeklenmedikHata);
            }
        }

    }
}
