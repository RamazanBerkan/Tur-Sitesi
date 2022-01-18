using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Interfaces.Repositories;
using ArgedeSP.Contracts.Models.Common;
using ArgedeSP.DAL.DataContext;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.DAL.Repositories
{
    public class UrunRepository : GenericRepository<Contracts.Entities.Urun>, IUrunRepository
    {
        private ArgedeSPContext argedeSPContext;
        public UrunRepository(ArgedeSPContext context)
            : base(context)
        {
            argedeSPContext = context;
        }

        public VeriListeleme SayfalaAramaIle(int sayfa, int sayfaBoyutu, int id, string urunAdi, string kategoriAdi, Durum durum)
        {
            try
            {
                IQueryable<Urun> query = argedeSPContext.Urunler.Include(x => x.UrunKategori);
                VeriListeleme veriListeleme = new VeriListeleme();

                if (id != 0)
                {
                    veriListeleme.Veri = query.Where(x => x.Id == id).ToList();
                    veriListeleme.ToplamVeri = 1;

                    return veriListeleme;
                }

                if (!string.IsNullOrWhiteSpace(urunAdi))
                {
                    query = query.Where(x => x.UrunAdi.Contains(urunAdi));
                }
                if (!string.IsNullOrWhiteSpace(kategoriAdi))
                {
                    query = query.Where(x => x.UrunKategori.Ad.ToLower().Contains(kategoriAdi.ToLower().Trim()));
                }

                if (durum != Durum.Yok)
                {
                    query = query.Where(x => x.Durum == durum);
                }

                veriListeleme.ToplamVeri = query.Count();
                veriListeleme.Veri = query.OrderBy(x => x.Dil).ThenByDescending(x=>x.OlusturmaTarihi).Skip((sayfa - 1) * sayfaBoyutu).Take(sayfaBoyutu).ToList();

                return veriListeleme;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{nameof(SayfalaAramaIle)} fonksiyonunda hata", new { sayfa, sayfaBoyutu, urunAdi, kategoriAdi });
                return null;
            }
        }

    }
}
