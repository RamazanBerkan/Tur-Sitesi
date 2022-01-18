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
    public class BlogRepository : GenericRepository<Contracts.Entities.Blog>, IBlogRepository
    {
        private ArgedeSPContext argedeSPContext;
        public BlogRepository(ArgedeSPContext context)
            : base(context)
        {
            argedeSPContext = context;
        }

        public VeriListeleme SayfalaAramaIle(int sayfa, int sayfaBoyutu, int id, string baslik, Dil dil, string seoUrl, string bkategori)
        {
            try
            {
                IQueryable<Blog> query = argedeSPContext.Bloglar.Include(x => x.BlogKategori);
                VeriListeleme veriListeleme = new VeriListeleme();

                if (id != 0)
                {
                    veriListeleme.Veri = query.Where(x => x.Id == id).ToList();
                    veriListeleme.ToplamVeri = 1;

                    return veriListeleme;
                }

                if (!string.IsNullOrWhiteSpace(baslik))
                {
                    query = query.Where(x => x.Baslik.Contains(baslik));
                }
                if (dil != Dil.Yok)
                {
                    query = query.Where(x => x.Dil == dil);
                }
                if (!string.IsNullOrWhiteSpace(seoUrl))
                {
                    query = query.Where(x => x.SeoUrl.ToLower().Contains(seoUrl.ToLower().Trim()));
                }
                if (!string.IsNullOrWhiteSpace(bkategori))
                {
                    query = query.Where(x => x.BlogKategori.BlogKategoriAdi.ToLower().Contains(bkategori.ToLower().Trim()));
                }

                veriListeleme.ToplamVeri = query.Count();
                veriListeleme.Veri = query.Skip((sayfa - 1) * sayfaBoyutu).OrderBy(x => x.Dil).ThenByDescending(x => x.OlusturmaTarihi).Take(sayfaBoyutu).ToList();

                return veriListeleme;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{nameof(SayfalaAramaIle)} fonksiyonunda hata", new { sayfa, sayfaBoyutu, baslik, id });
                return null;
            }
        }
    }
}
