using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Interfaces.Repositories;
using ArgedeSP.Contracts.Models.Common;
using ArgedeSP.DAL.DataContext;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.DAL.Repositories
{
    public class BlogKategoriRepository : GenericRepository<Contracts.Entities.BlogKategori>, IBlogKategoriRepository
    {
        private ArgedeSPContext argedeSPContext;
        public BlogKategoriRepository(ArgedeSPContext context)
            : base(context)
        {
            argedeSPContext = context;
        }

        public VeriListeleme SayfalaAramaIle(int sayfa, int sayfaBoyutu, int id, string blogkategoriadi, string seoUrl, Dil dil)
        {
            try
            {
                IQueryable<BlogKategori> query = argedeSPContext.BlogKategorileri;
                VeriListeleme veriListeleme = new VeriListeleme();

                if (id != 0)
                {
                    veriListeleme.Veri = query.Where(x => x.Id == id).ToList();
                    veriListeleme.ToplamVeri = 1;

                    return veriListeleme;
                }

                if (!string.IsNullOrWhiteSpace(blogkategoriadi))
                {
                    query = query.Where(x => x.BlogKategoriAdi.Contains(blogkategoriadi));
                }
                if (dil != Dil.Yok)
                {
                    query = query.Where(x => x.Dil == dil);
                }
                if (!string.IsNullOrWhiteSpace(seoUrl))
                {
                    query = query.Where(x => x.SeoUrl.ToLower().Contains(seoUrl.ToLower().Trim()));
                }

                veriListeleme.ToplamVeri = query.Count();
                veriListeleme.Veri = query.Skip((sayfa - 1) * sayfaBoyutu).Take(sayfaBoyutu).ToList();

                return veriListeleme;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{nameof(SayfalaAramaIle)} fonksiyonunda hata", new { sayfa, sayfaBoyutu, blogkategoriadi, id });
                return null;
            }
        }
    }
}
