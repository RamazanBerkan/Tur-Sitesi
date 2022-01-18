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
    public class HaberRepository : GenericRepository<Contracts.Entities.Haber>, IHaberRepository
    {
        private ArgedeSPContext argedeSPContext;
        public HaberRepository(ArgedeSPContext context)
            : base(context)
        {
            argedeSPContext = context;
        }

        public VeriListeleme SayfalaAramaIle(int sayfa, int sayfaBoyutu, int id, string baslik, Dil dil)
        {
            try
            {
                IQueryable<Haber> query = argedeSPContext.Haberler;
                VeriListeleme veriListeleme = new VeriListeleme();

                if (id != 0)
                {
                    veriListeleme.Veri = query.Where(x => x.Id == id).ToList();
                    veriListeleme.ToplamVeri = 1;

                    return veriListeleme;
                }

                if (dil != Dil.Yok)
                {
                    query = query.Where(x => x.Dil == dil);
                }

                if (!string.IsNullOrWhiteSpace(baslik))
                {
                    query = query.Where(x => x.Baslik.Trim().ToLower().Contains(baslik.Trim().ToLower()));
                }


                veriListeleme.ToplamVeri = query.Count();
                veriListeleme.Veri = query.Skip((sayfa - 1) * sayfaBoyutu).Take(sayfaBoyutu).OrderBy(x => x.Dil).ToList();

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
