using ArgedeSP.Contracts.Models.Common;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.WebUI.Helpers
{
    public static class Helpers
    {
        public static string TimeAgo(this DateTime dateTime)
        {
            string result = string.Empty;
            var timeSpan = DateTime.Now.Subtract(dateTime);

            if (timeSpan < TimeSpan.FromSeconds(10))
            {
                result = string.Format("şimdi");
            }
            else if (timeSpan <= TimeSpan.FromSeconds(60) && timeSpan > TimeSpan.FromSeconds(10))
            {
                result = string.Format("{0} saniye önce", timeSpan.Seconds);
            }
            else if (timeSpan <= TimeSpan.FromMinutes(60))
            {
                result = timeSpan.Minutes > 1 ?
                    String.Format("{0} dakika önce", timeSpan.Minutes) :
                    "Bir dakika önce";
            }
            else if (timeSpan <= TimeSpan.FromHours(24))
            {
                result = timeSpan.Hours > 1 ?
                    String.Format("{0} saat önce", timeSpan.Hours) :
                    "Bir saat önce";
            }
            else if (timeSpan <= TimeSpan.FromDays(30))
            {
                result = timeSpan.Days > 1 ?
                    String.Format("{0} gün önce", timeSpan.Days) :
                    "Dün";
            }
            else if (timeSpan <= TimeSpan.FromDays(365))
            {
                result = timeSpan.Days > 30 ?
                    String.Format("{0} ay önce", timeSpan.Days / 30) :
                    "Bir ay önce";
            }
            else
            {
                result = timeSpan.Days > 365 ?
                    String.Format("{0} yıl önce", timeSpan.Days / 365) :
                    "Bir yıl önce";
            }

            return result;
        }

        public static List<string> GetErrorListFromModelState
                                              (ModelStateDictionary modelState)
        {
            var query = from state in modelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;

            var errorList = query.ToList();
            return errorList;
        }

        public static string BankaYaziyaCevir(this Banka banka)
        {
            switch (banka)
            {
                case Banka.Garanti:
                    return "Garanti";
                case Banka.FinansBank:
                    return "Finans Bank";
                case Banka.YapiKredi:
                    return "Yapı Kredi";
                case Banka.BankAsya:
                    return "Bank Asya";
                case Banka.IsBankasi:
                    return "İş Bankası";
                case Banka.HalkBankasi:
                    return "Halk Bankasi";
                case Banka.AkBank:
                    return "Ak Bank";
                case Banka.ZiraatBankasi:
                    return "Ziraat Bankası";
                case Banka.Teb:
                    return "Teb Bankası";
                case Banka.IngBank:
                    return "Ing Bank";
                case Banka.SekerBank:
                    return "Şeker Bank";
                case Banka.AnadoluBank:
                    return "Anadolu Bank";
                case Banka.CardPlus:
                    return "Card Plus";
                case Banka.AktifBank:
                    return "Aktif Bank";
                case Banka.Abank:
                    return "A Bank";
                case Banka.Yok:
                default:
                    return "Yok";
            }
        }
        public static string SiparisDurumuYaziyaCevir(this SiparisDurumu siparisDurumu)
        {
            switch (siparisDurumu)
            {
                case SiparisDurumu.Aktif:
                    return "Aktif";
                case SiparisDurumu.Hazirlandi:
                    return "Hazırlandı";
                case SiparisDurumu.Hazirlaniyor:
                    return "Hazırlanıyor";
                case SiparisDurumu.IptalEdildi:
                    return "İptal Edildi";
                case SiparisDurumu.KargoyaVerildi:
                    return "Kargoya Verildi";
                case SiparisDurumu.OdemeBekliyor:
                    return "Ödeme Bekliyor";
                case SiparisDurumu.OnayBekliyor:
                    return "Onay Bekliyor";
                case SiparisDurumu.Onaylandi:
                    return "Onaylandı";
                case SiparisDurumu.Onaylanmadi:
                    return "Onaylanmadı";
                case SiparisDurumu.Tamamlandi:
                    return "Tamamlandı";
                case SiparisDurumu.Yok:
                default:
                    return "Yok";
            }
        }
        public static string OdemeYontemiYaziyaCevir(this OdemeYontemi odemeYontemi)
        {
            switch (odemeYontemi)
            {
                case OdemeYontemi.Havale:
                    return "Havale";
                case OdemeYontemi.KapidaOdeme:
                    return "Kapıda Ödeme";
                case OdemeYontemi.KrediKarti:
                    return "Kredi Kartı";
                case OdemeYontemi.Yok:
                default:
                    return "Yok";
            }
        }
    }

}
