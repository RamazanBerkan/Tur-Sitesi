using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Models.Common;

namespace ArgedeSP.Contracts.Interfaces.Repositories
{
    public interface IReferansRepository : IGenericRepository<Entities.Referans>
    {
        VeriListeleme SayfalaAramaIle(int sayfa, int sayfaBoyutu, int id, string ad,string seoUrl);
    }
}
