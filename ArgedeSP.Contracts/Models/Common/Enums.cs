using System;
using System.Collections.Generic;
using System.Text;

namespace ArgedeSP.Contracts.Models.Common
{
    public class Enums
    {
        public enum MesajKodu
        {
            #region Sistem Hataları
            Bos = 1,

            BeklenmedikHata = 1001,
            ParametrelerHatali = 1002,
            NesneBulunamadi = 1003,
            #endregion

            #region Kullanıcı Hataları
            KullaniciBulunamadi = 2002,
            KullaniciAdiZatenVar = 2003,
            KullaniciEklenemedi = 2004,
            KullaniciEmailiZatenVar=2005,
            RolEklenemedi=2006,
            SifreZorunlu=2007,
            #endregion

            #region ProjeKategori
            ProjeKategoriBulunamadi = 3001,
            ProjeKategoriGuncellendi = 3002,
            #endregion

            #region Proje
            ProjeBulunamadi = 4001,
            ProjeGuncellendi = 4002,
            #endregion

            #region Referans
            ReferansBulunamadi = 6001,
            ReferansGuncellendi = 6002,
            #endregion

            #region HizmetKategori
            HizmetKategoriBulunamadi = 7001,
            HzmetKategoriGuncellendi = 7002,
            #endregion

            #region Hizmet
            HizmetBulunamadi = 8001,
            HizmetGuncellendi = 8002,
            #endregion


            #region Ekip
            ElemanBulunamadi = 9001,
            ElemanGuncellendi = 9002,
            #endregion

            #region BlogKategori
            BlogKategoriBulunamadı = 10001,
            BlogKategoriGuncellendi = 10002,
            #endregion

            #region Blog
            BlogBulunamadı = 11001,
            BlogGuncellendi = 11002,
            #endregion

            #region UrunKategori
            UrunKategoriBulunamadı = 12001,
            UrunKategoriGuncellendi = 12002,
            #endregion

            #region Urun
            UrunBulunamadi = 13001,
            UrunGuncellendi = 13002,
            StokAsildi = 13003,
            UrunSipariseEkli = 13004,
            #endregion

            #region UrunTipi
            UrunTipiBulunamadi = 16001,
            #endregion

            #region Sepet
            SepetBulunamadi = 14001,
            #endregion

            #region Olay
            OlayBulunamadi = 17001,
            OlayGuncellendi = 17002,
            #endregion

            #region Slider
            SliderBulunamadi=18001,
            #endregion

            #region Haberler
            HaberBulunamadi=19001,
            #endregion

            #region Sana Pos
            SanalPosBulunamadi = 15001,
            #endregion

            #region Yurt Hizmeti
            YurtHizmetiBulunamadi=16001,
            BuAracKoduZatenKayitli=16002,
            YurtHizmetiAraciBulunamadi=16003,
            #endregion

            #region Randevu
            RandevuSayisiAsildi=17001,
            RandevuBulunamadi=17002,
            RandevuyuBaskasiAlmis=17003,
            #endregion

            #region ortak
            SeoUrlZatenVar = 5001
            #endregion


        }

        public enum Durum : int
        {
            Yok = 0,
            Aktif = 1,
            Pasif = 2,
            Silindi = 3,
        }

        public enum SiparisDurumu : int
        {
            Yok = 0,
            Onaylanmadi = 12,
            Onaylandi = 1,
            Hazirlaniyor = 2,
            Hazirlandi = 3,
            KargoyaVerildi = 6,
            IptalEdildi = 7,
            OnayBekliyor = 8,
            Aktif = 9,
            Tamamlandi = 10,
            OdemeBekliyor = 11,

        }

        public enum OdemeYontemi : int
        {

            Yok = 0,
            KrediKarti = 1,
            KapidaOdeme = 2,
            Havale = 3,
        }

        public enum Banka : int
        {
            Garanti = 1,
            FinansBank = 2,
            YapiKredi = 3,
            BankAsya = 4,
            IsBankasi = 5,
            HalkBankasi = 6,
            AkBank = 7,
            ZiraatBankasi = 8,
            Teb = 9,
            IngBank = 10,
            SekerBank = 11,
            AnadoluBank = 12,
            CardPlus = 13,
            AktifBank = 14,
            Abank = 15,

            Yok = 99
        }

        public enum Dil : int
        {
            Yok = 0,
            Turkce = 1,
            Ingilizce = 2,
            Fransizca = 3,
            Rusca = 4,
            Norvecce = 5,
            Italyanca = 6,
            Portekizce = 7,
            Japonca = 8,
            Korece = 9,
            Kurtce = 10,
            Arabca = 11

        }

        public enum SliderYeri
        {
            AnasayfaSlider = 0,
            AnasayfaBanner,
        }

        public enum Tip : int
        {
            Blog = 1,
            Haber = 2
        }

        public enum ProjeDurumu : int
        {
            Tamamlandi = 0,
            Devameden = 1,

        }
    }
}
