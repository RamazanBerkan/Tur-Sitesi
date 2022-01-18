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
using System.Threading.Tasks;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.DAL.Repositories
{
    public class HizmetRepository : GenericRepository<Contracts.Entities.Hizmet>, IHizmetRepository
    {
        private ArgedeSPContext argedeSPContext;
        public HizmetRepository(ArgedeSPContext context)
            : base(context)
        {
            argedeSPContext = context;
        }

        public VeriListeleme SayfalaAramaIle(int sayfa, int sayfaBoyutu, int id, string ad, string seoUrl, Dil dil)
        {
            try
            {
                IQueryable<Hizmet> query = argedeSPContext.Hizmetler;
                VeriListeleme veriListeleme = new VeriListeleme();

                if (id != 0)
                {
                    veriListeleme.Veri = query.Where(x => x.Id == id).ToList();
                    veriListeleme.ToplamVeri = 1;

                    return veriListeleme;
                }

                if (!string.IsNullOrWhiteSpace(ad))
                {
                    query = query.Where(x => x.HizmetAdi.Contains(ad));
                }
                if (!string.IsNullOrWhiteSpace(seoUrl))
                {
                    query = query.Where(x => x.SeoUrl.ToLower().Contains(seoUrl.ToLower().Trim()));
                }

                if (dil != Dil.Yok)
                {
                    query = query.Where(x => x.Dil == dil);
                }

                veriListeleme.ToplamVeri = query.Count();
                veriListeleme.Veri = query.Skip((sayfa - 1) * sayfaBoyutu).Take(sayfaBoyutu).ToList();

                return veriListeleme;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{nameof(SayfalaAramaIle)} fonksiyonunda hata", new { sayfa, sayfaBoyutu, ad, id });
                return null;
            }
        }
    }
}
