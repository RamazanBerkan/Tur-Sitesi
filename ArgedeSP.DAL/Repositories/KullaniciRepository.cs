using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Interfaces.Repositories;
using ArgedeSP.Contracts.Models.Common;
using ArgedeSP.Contracts.Models.DTO.Kulanici.Req;
using ArgedeSP.DAL.DataContext;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static ArgedeSP.Contracts.Models.Common.Enums;
using X.PagedList;

namespace ArgedeSP.DAL.Repositories
{
    public class KullaniciRepository : GenericRepository<Contracts.Entities.Kullanici>, IKullaniciRepository
    {

        private ArgedeSPContext argedeSPContext;
        public KullaniciRepository(ArgedeSPContext context)
            : base(context)
        {
            argedeSPContext = context;
        }

        public OperationResult SayfalaAramaIle(KullaniciListele_REQ kullaniciListele_REQ)
        {
            try
            {
                IQueryable<Kullanici> query = argedeSPContext.Kullanicilar
                   .Include(x => x.KullaniciRolleri)
                   .ThenInclude(x => x.Role).OrderByDescending(x => x.OlusturmaTarihi);

                VeriListeleme veriListeleme = new VeriListeleme();

                if (!string.IsNullOrWhiteSpace(kullaniciListele_REQ.Id))
                {
                    veriListeleme.Veri = query.Where(x => x.Id == kullaniciListele_REQ.Id).ToList();
                    veriListeleme.ToplamVeri = 1;

                    return OperationResult.Success(veriListeleme);
                }

                if (!string.IsNullOrWhiteSpace(kullaniciListele_REQ.Ad))
                {
                    query = query.Where(x => x.Ad.Contains(kullaniciListele_REQ.Ad.Trim()) || x.Soyad.Contains(kullaniciListele_REQ.Ad.Trim()));
                }

                if (!string.IsNullOrWhiteSpace(kullaniciListele_REQ.Email))
                {
                    query = query.Where(x => x.Email.Contains(kullaniciListele_REQ.Email.Trim()));
                }

                if (!string.IsNullOrWhiteSpace(kullaniciListele_REQ.UserName))
                {
                    query = query.Where(x => x.UserName.Contains(kullaniciListele_REQ.UserName.Trim()));
                }


                if (!string.IsNullOrWhiteSpace(kullaniciListele_REQ.PhoneNumber))
                {
                    query = query.Where(x => x.PhoneNumber.Contains(kullaniciListele_REQ.PhoneNumber.Trim()));
                }


                if (!string.IsNullOrWhiteSpace(kullaniciListele_REQ.TCKN))
                {
                    query = query.Where(x => x.TCKN.Contains(kullaniciListele_REQ.TCKN.Trim()));
                }


                if (kullaniciListele_REQ.Durum.HasValue)
                {
                    query = query.Where(x => x.Durum == kullaniciListele_REQ.Durum.Value);
                }
                veriListeleme.ToplamVeri = query.Count();

                if (kullaniciListele_REQ.Sayfa.HasValue)
                {
                    veriListeleme.Veri = query.ToPagedList(kullaniciListele_REQ.Sayfa.Value,kullaniciListele_REQ.SayfaBoyutu.Value);
                }
                else
                {
                    veriListeleme.Veri = query.Take(kullaniciListele_REQ.SayfaBoyutu.Value).ToPagedList();
                }

                return OperationResult.Success(veriListeleme);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{nameof(SayfalaAramaIle)} fonksiyonunda hata", kullaniciListele_REQ);
                return OperationResult.Error(MesajKodu.BeklenmedikHata);

            }
        }


    }
}