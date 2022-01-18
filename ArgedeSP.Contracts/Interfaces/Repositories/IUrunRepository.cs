using ArgedeSP.Contracts.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.Contracts.Interfaces.Repositories
{
    public interface IUrunRepository : IGenericRepository<Entities.Urun>
    {
        VeriListeleme SayfalaAramaIle(int sayfa, int sayfaBoyutu, int id, string urunAdi, string kategoriAdi, Durum durum);
    }
}
