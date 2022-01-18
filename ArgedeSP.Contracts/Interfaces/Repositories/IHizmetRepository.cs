using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Models.Common;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.Contracts.Interfaces.Repositories
{
    public interface IHizmetRepository : IGenericRepository<Entities.Hizmet>
    {
        VeriListeleme SayfalaAramaIle(int sayfa, int sayfaBoyutu, int id, string ad,string seoUrl,Dil dil);
    }
}
