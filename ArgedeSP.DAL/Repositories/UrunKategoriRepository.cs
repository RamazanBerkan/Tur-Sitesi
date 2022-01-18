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

namespace ArgedeSP.DAL.Repositories
{
    public class UrunKategoriRepository : GenericRepository<Contracts.Entities.UrunKategori>, IUrunKategoriRepository
    {
        private ArgedeSPContext argedeSPContext;
        public UrunKategoriRepository(ArgedeSPContext context)
            : base(context)
        {
            argedeSPContext = context;
        }

        public VeriListeleme SayfalaAramaIle(int sayfa, int sayfaBoyutu, int id, string ad, string seoUrl)
        {
            try
            {
                IQueryable<UrunKategori> query = argedeSPContext.UrunKategorileri;
                VeriListeleme veriListeleme = new VeriListeleme();

                if (id != 0)
                {
                    veriListeleme.Veri = query.Where(x => x.Id == id).ToList();
                    veriListeleme.ToplamVeri = 1;

                    return veriListeleme;
                }

                if (!string.IsNullOrWhiteSpace(ad))
                {
                    query = query.Where(x => x.Ad.Contains(ad));
                }
                if (!string.IsNullOrWhiteSpace(seoUrl))
                {
                    query = query.Where(x => x.SeoUrl.ToLower().Contains(seoUrl.ToLower().Trim()));
                }

                veriListeleme.ToplamVeri = query.Count();
                veriListeleme.Veri = query.OrderBy(x => x.Dil).ThenByDescending(x => x.OlusturmaTarihi).Skip((sayfa - 1) * sayfaBoyutu).Take(sayfaBoyutu).ToList();

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
