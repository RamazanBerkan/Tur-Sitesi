using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.Contracts.Helpers
{
    public static class Global
    {

        public static IEnumerable Errors(this ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return modelState.ToDictionary(kvp => kvp.Key,
                    kvp => kvp.Value.Errors
                                    .Select(e => e.ErrorMessage).ToArray())
                                    .Where(m => m.Value.Any());
            }
            return null;
        }

        public static string GetSHA1(string SHA1Data)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            string HashedPassword = SHA1Data;
            byte[] hashbytes = Encoding.GetEncoding("ISO-8859-9").GetBytes(HashedPassword);
            byte[] inputbytes = sha.ComputeHash(hashbytes);
            return GetHexaDecimal(inputbytes);
        }

        public static string GetHexaDecimal(byte[] bytes)
        {
            StringBuilder s = new StringBuilder();
            int length = bytes.Length;
            for (int n = 0; n <= length - 1; n++)
            {
                s.Append(String.Format("{0,2:x}", bytes[n]).Replace(" ", "0"));
            }
            return s.ToString();
        }



        public static string AssecoApiServerUrl(this Banka id)
        {
            var retval = "";

            if (id == Banka.IsBankasi) retval = "https://sanalpos.isbank.com.tr/fim/api";
            if (id == Banka.FinansBank) retval = "https://www.fbwebpos.com/fim/api";
            if (id == Banka.AkBank) retval = "https://www.sanalakpos.com/fim/api";
            if (id == Banka.HalkBankasi) retval = "https://sanalpos.halkbank.com.tr/fim/api";
            if (id == Banka.ZiraatBankasi) retval = "https://sanalpos2.ziraatbank.com.tr/fim/api";
            if (id == Banka.Teb) retval = "https://sanalpos.teb.com.tr/fim/api";
            if (id == Banka.IngBank) retval = "https://sanalpos.ingbank.com.tr/fim/api";
            if (id == Banka.SekerBank) retval = "https://sanalpos.sekerbank.com.tr/fim/api";
            if (id == Banka.AnadoluBank) retval = "https://anadolusanalpos.est.com.tr/fim/api";
            if (id == Banka.CardPlus) retval = "https://sanalpos.card-plus.net/fim/api";
            if (id == Banka.AktifBank) retval = "https://sanalpos.aktifbank.com.tr/fim/api";
            if (id == Banka.Abank) retval = "https://sanalpos.abank.com.tr/fim/api";

            return retval;
        }


        public static string Asseco3DSecureGateUrl(this Banka id)
        {
            var retval = "";

            if (id == Banka.IsBankasi) retval = "https://sanalpos.isbank.com.tr/fim/est3Dgate";
            if (id == Banka.FinansBank) retval = "https://www.fbwebpos.com/fim/est3Dgate";
            if (id == Banka.AkBank) retval = "https://www.sanalakpos.com/fim/est3Dgate";
            if (id == Banka.HalkBankasi) retval = "https://sanalpos.halkbank.com.tr/fim/est3Dgate";
            if (id == Banka.ZiraatBankasi) retval = "https://sanalpos2.ziraatbank.com.tr/fim/est3Dgate";
            if (id == Banka.Teb) retval = "https://sanalpos.teb.com.tr/fim/est3Dgate";
            if (id == Banka.IngBank) retval = "https://sanalpos.ingbank.com.tr/fim/est3Dgate";
            if (id == Banka.SekerBank) retval = "https://sanalpos.sekerbank.com.tr/fim/est3Dgate";
            if (id == Banka.AnadoluBank) retval = "https://anadolusanalpos.est.com.tr/fim/est3Dgate";
            if (id == Banka.CardPlus) retval = "https://sanalpos.card-plus.net/fim/est3Dgate";
            if (id == Banka.AktifBank) retval = "https://sanalpos.aktifbank.com.tr/fim/est3Dgate";
            if (id == Banka.Abank) retval = "https://sanalpos.abank.com.tr/fim/est3Dgate";

            return retval;
        }

        public static Dil DilGetir(this CultureInfo culture)
        {
            switch (culture.Name)
            {
                case "tr-TR":
                    return Dil.Turkce;
                case "en-US":
                    return Dil.Ingilizce;
                case "ru-RU":
                    return Dil.Rusca;
                case "nb-NO":
                    return Dil.Norvecce;
                default:
                    return Dil.Turkce;
            }
        }

        public static string DilYaziyaCevir(this Dil dil)
        {
            switch (dil)
            {
                case Dil.Turkce:
                    return "Türkçe";
                case Dil.Ingilizce:
                    return "İngilizce";
                default:
                    return "Türkçe";
            }
        }

        public static string Kisalt(this string icerik, int Ksayi)
        {
            if (Ksayi < icerik.Length)
            {
                return icerik.Substring(0, Ksayi);
            }

            return icerik;
        }
        public static DateTime? ParseDateTime(
            string dateToParse,
            string[] formats = null,
            IFormatProvider provider = null,
            DateTimeStyles styles = DateTimeStyles.None)
        {
            var CUSTOM_DATE_FORMATS = new string[]
                {
                "yyyyMMddTHHmmssZ",
                "yyyyMMddTHHmmZ",
                "yyyyMMddTHHmmss",
                "yyyyMMddTHHmm",
                "yyyyMMddHHmmss",
                "yyyyMMddHHmm",
                "yyyyMMdd",
                "yyyy-MM-ddTHH-mm-ss",
                "yyyy-MM-dd-HH-mm-ss",
                "yyyy-MM-dd-HH-mm",
                "yyyy-MM-dd",
                "MM-dd-yyyy"
                };

            if (formats == null || !formats.Any())
            {
                formats = CUSTOM_DATE_FORMATS;
            }

            DateTime validDate;

            foreach (var format in formats)
            {
                if (format.EndsWith("Z"))
                {
                    if (DateTime.TryParseExact(dateToParse, format,
                             provider,
                             DateTimeStyles.AssumeUniversal,
                             out validDate))
                    {
                        return validDate;
                    }
                }

                if (DateTime.TryParseExact(dateToParse, format,
                         provider, styles, out validDate))
                {
                    return validDate;
                }
            }

            return null;
        }

        public static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }

    }
}
