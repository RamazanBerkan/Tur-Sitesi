using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Models.Common;
using ArgedeSP.Contracts.Models.DTO.Kulanici.Req;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ArgedeSP.Contracts.Interfaces.Repositories
{
    public interface IKullaniciRepository : IGenericRepository<Entities.Kullanici>
    {
        OperationResult SayfalaAramaIle(KullaniciListele_REQ kullaniciListele_REQ);
      
    }
}
